using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class NaughtyInspector : UnityEditor.Editor
    {
        private List<SerializedProperty> _serializedProperties = new List<SerializedProperty>();
        private IEnumerable<FieldInfo> _nonSerializedFields;
        private IEnumerable<PropertyInfo> _nativeProperties;
        private IEnumerable<MethodInfo> _methods;
        private Dictionary<string, IEnumerable<MethodInfo>> _groupedMethods;
        private IEnumerable<Attribute> _classAttrs;
        private Dictionary<string, SavedBool> _foldouts = new Dictionary<string, SavedBool>();
        private Dictionary<string, SavedBool> _groupedFoldouts = new Dictionary<string, SavedBool>();
        private SavedInt _selectedTab;

        protected virtual void OnEnable()
        {
            _classAttrs = ReflectionUtility.GetAllClassAttributes(target)
                .GroupBy(x => x.GetType().BaseType)
                .Select(x => x.First());

            _nonSerializedFields = ReflectionUtility.GetAllFields(
                target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0);

            _nativeProperties = ReflectionUtility.GetAllProperties(
                target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0);

            _methods = ReflectionUtility.GetAllMethods(
                target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

            var tempMethods = ReflectionUtility.GetAllMethods(target,
                m => m.GetCustomAttributes(typeof(ButtonGroupAttribute), true).Length > 0);
            _groupedMethods = new Dictionary<string, IEnumerable<MethodInfo>>();
            foreach (var m in tempMethods)
            {
                if (!_groupedMethods.ContainsKey(m.GetCustomAttribute<ButtonGroupAttribute>().Group))
                {
                    _groupedMethods[m.GetCustomAttribute<ButtonGroupAttribute>().Group] = new List<MethodInfo>();
                }

                _groupedMethods[m.GetCustomAttribute<ButtonGroupAttribute>().Group] =
                    _groupedMethods[m.GetCustomAttribute<ButtonGroupAttribute>().Group].Append(m);
            }
        }

        protected virtual void OnDisable()
        {
            ReorderableListPropertyDrawer.Instance.ClearCache();
            SerializedCollectionPropertyDrawer.Instance.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            DrawClassAvatar();

            GetSerializedProperties(ref _serializedProperties);

            bool anyNaughtyAttribute =
                _serializedProperties.Any(p => PropertyUtility.GetAttribute<INaughtyAttribute>(p) != null);
            if (!anyNaughtyAttribute)
            {
                DrawDefaultInspector();
            }
            else
            {
                DrawSerializedProperties();
            }

            DrawNonSerializedFields();
            DrawNativeProperties();
            DrawButtons();
        }

        protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
        {
            outSerializedProperties.Clear();
            using (var iterator = serializedObject.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
                    } while (iterator.NextVisible(false));
                }
            }
        }

        protected void DrawClassAvatar()
        {
            foreach (var clazzAttr in _classAttrs)
            {
                if (clazzAttr is ClassAvatarAttribute)
                {
                    var classAvatar = (ClassAvatarAttribute)clazzAttr;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(
                        new GUIContent((Texture)EditorGUIUtility.Load(classAvatar.Path)),
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f),
                        GUILayout.Width(EditorGUIUtility.singleLineHeight * 2f));
                    EditorGUILayout.LabelField(
                        new GUIContent(classAvatar.Message),
                        GetAvatarGUIStyle(),
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f));
                    EditorGUILayout.EndHorizontal();
                    NaughtyEditorGUI.HorizontalLine(EditorGUILayout.GetControlRect(false),
                        HorizontalLineAttribute.DefaultHeight,
                        HorizontalLineAttribute.DefaultColor.GetColor());
                }
            }
        }

        protected void DrawSerializedProperties()
        {
            serializedObject.Update();

            // Draw non-grouped serialized properties
            foreach (var property in GetNonGroupedProperties(_serializedProperties))
            {
                if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
                {
                    using (new EditorGUI.DisabledScope(disabled: true))
                    {
                        EditorGUILayout.PropertyField(property);
                    }
                }
                else
                {
                    NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
                }
            }

            var tabGroups = GetTabGroupedProperties(_serializedProperties).ToArray();

            if (tabGroups.Length > 0)
            {
                if (null == _selectedTab)
                {
                    _selectedTab = new SavedInt($"{target.GetInstanceID()}.tab.selected", 0);
                }

                _selectedTab.Value = NaughtyEditorGUI.TabGroup_Tabs(_selectedTab.Value, tabGroups);
            }

            // Draw grouped serialized properties
            for (var i = 0; i < tabGroups.Length; i++)
            {
                if (i != _selectedTab.Value) continue;

                var group = tabGroups[i];
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                // TabGroup does not require label below the line.
                NaughtyEditorGUI.BeginBoxGroup_Layout();
                foreach (var property in visibleProperties)
                {
                    NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
                }

                NaughtyEditorGUI.EndBoxGroup_Layout();
            }

            // Draw grouped serialized properties
            foreach (var group in GetBoxGroupedProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                NaughtyEditorGUI.BeginBoxGroup_Layout(group.Key);
                foreach (var property in visibleProperties)
                {
                    NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
                }

                NaughtyEditorGUI.EndBoxGroup_Layout();
            }

            // Draw foldout serialized properties
            foreach (var group in GetFoldoutProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                if (!_foldouts.ContainsKey(group.Key))
                {
                    _foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
                }

                _foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
                if (_foldouts[group.Key].Value)
                {
                    foreach (var property in visibleProperties)
                    {
                        NaughtyEditorGUI.PropertyField_Layout(property, true);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawNonSerializedFields(bool drawHeader = false)
        {
            if (_nonSerializedFields.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
                        HorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var field in _nonSerializedFields)
                {
                    NaughtyEditorGUI.NonSerializedField_Layout(serializedObject.targetObject, field);
                }
            }
        }

        protected void DrawNativeProperties(bool drawHeader = false)
        {
            if (_nativeProperties.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
                        HorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var property in _nativeProperties)
                {
                    NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, property);
                }
            }
        }

        protected void DrawButtons(bool drawHeader = false)
        {
            if (_methods.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
                        HorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var method in _methods)
                {
                    NaughtyEditorGUI.Button(serializedObject.targetObject, method);
                }
            }

            if (_groupedMethods.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Grouped Buttons", GetGroupGUIStyle());
                    NaughtyEditorGUI.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
                        HorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var group in _groupedMethods)
                {
                    EditorGUILayout.Space();
                    if (!_groupedFoldouts.ContainsKey(group.Key))
                    {
                        _groupedFoldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", true);
                    }

                    EditorGUILayout.BeginVertical(GetGroupGUIStyle());
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(group.Key, GetGroupedHeaderGUIStyle());
                    if (GUILayout.Button(_groupedFoldouts[group.Key].Value ? "Hide" : "Expand"))
                    {
                        _groupedFoldouts[group.Key].Value = !_groupedFoldouts[group.Key].Value;
                    }

                    EditorGUILayout.EndHorizontal();

                    if (_groupedFoldouts[group.Key].Value)
                    {
                        NaughtyEditorGUI.HorizontalLine(
                            EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight,
                            HorizontalLineAttribute.DefaultColor.GetColor());

                        foreach (var method in group.Value)
                        {
                            NaughtyEditorGUI.GroupedButton(serializedObject.targetObject, method);
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
            }
        }

        private static IEnumerable<SerializedProperty> GetNonGroupedProperties(
            IEnumerable<SerializedProperty> properties)
        {
            return properties.Where(p => PropertyUtility.GetAttribute<IGroupAttribute>(p) == null);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetTabGroupedProperties(
            IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(p => PropertyUtility.GetAttribute<TabGroupAttribute>(p) != null)
                .OrderByDescending(p => PropertyUtility.GetAttribute<TabGroupAttribute>(p).SortOrder)
                .GroupBy(p => PropertyUtility.GetAttribute<TabGroupAttribute>(p).Name);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetBoxGroupedProperties(
            IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) != null)
                .OrderByDescending(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).SortOrder)
                .GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).Name);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetFoldoutProperties(
            IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p) != null)
                .OrderByDescending(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).SortOrder)
                .GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).Name);
        }

        private static GUIStyle GetHeaderGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.UpperCenter;
            return style;
        }

        private static GUIStyle GetGroupedHeaderGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;
            return style;
        }

        private static GUIStyle GetGroupGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            return style;
        }

        private static GUIStyle GetAvatarGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.whiteLargeLabel);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 16;
            style.stretchHeight = true;

            return style;
        }
    }
}