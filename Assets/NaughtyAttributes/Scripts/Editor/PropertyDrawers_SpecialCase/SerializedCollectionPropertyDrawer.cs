namespace NaughtyAttributes.Editor
{
    using UnityEngine;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEditorInternal;
    using System.Collections;
    using System.Collections.Generic;

    public class SerializedCollectionPropertyDrawer : SpecialCasePropertyDrawerBase
    {
        public static readonly SerializedCollectionPropertyDrawer Instance = new SerializedCollectionPropertyDrawer();

        private readonly Dictionary<string, ReorderableList> _reorderableListsByPropertyName =
            new Dictionary<string, ReorderableList>();

        private GUIStyle _labelStyle;

        private GUIStyle GetLabelStyle()
        {
            if (_labelStyle == null)
            {
                _labelStyle = new GUIStyle(EditorStyles.boldLabel);
                _labelStyle.richText = true;
            }

            return _labelStyle;
        }

        private string GetPropertyKeyName(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetInstanceID() + "." + property.name;
        }

        protected override float GetPropertyHeight_Internal(SerializedProperty property)
        {
            if (property.isArray)
            {
                string key = GetPropertyKeyName(property);

                if (_reorderableListsByPropertyName.TryGetValue(key, out ReorderableList reorderableList) == false)
                {
                    return 0;
                }

                return reorderableList.GetHeight();
            }

            return EditorGUI.GetPropertyHeight(property, true);
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.isArray)
            {
                string key = GetPropertyKeyName(property);
                SerializedCollectionAttribute propTyped =
                    PropertyUtility.GetAttribute<SerializedCollectionAttribute>(property);

                var targetObject = property.serializedObject.targetObject;
                var targetObjectType = targetObject.GetType();
                var field = targetObjectType.GetField(propTyped.VariableName,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                if (null != field)
                {
                    var revealField = targetObjectType.GetField(property.propertyPath);
                    var refValueRaw = ((IEnumerable)field.GetValue(targetObject));
                    var strList = new List<string>();
                    foreach (var o in refValueRaw)
                    {
                        strList.Add(o.ToString());
                    }

                    revealField.SetValue(targetObject, strList);
                }

                ReorderableList reorderableList = null;
                if (!_reorderableListsByPropertyName.ContainsKey(key))
                {
                    reorderableList =
                        new ReorderableList(property.serializedObject, property, false, true, false, false)
                        {
                            drawHeaderCallback = (Rect r) =>
                            {
                                EditorGUI.LabelField(r,
                                    string.Format("{0}: {1}", propTyped.VariableName, property.arraySize),
                                    GetLabelStyle());
                            },

                            drawElementCallback = (Rect r, int index, bool isActive, bool isFocused) =>
                            {
                                SerializedProperty element = property.GetArrayElementAtIndex(index);
                                r.y += 1.0f;
                                r.x += 10.0f;
                                r.width -= 10.0f;

                                EditorGUI.BeginDisabledGroup(true);
                                EditorGUI.PropertyField(new Rect(r.x, r.y, r.width, EditorGUIUtility.singleLineHeight),
                                    element, true);
                                EditorGUI.EndDisabledGroup();
                            },

                            elementHeightCallback = (int index) =>
                            {
                                return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
                            }
                        };

                    _reorderableListsByPropertyName[key] = reorderableList;
                }

                reorderableList = _reorderableListsByPropertyName[key];

                if (rect == default)
                {
                    reorderableList.DoLayoutList();
                }
                else
                {
                    reorderableList.DoList(rect);
                }
            }
            else
            {
                string message = typeof(SerializedCollectionAttribute).Name + " can be used only on arrays or lists";
                NaughtyEditorGUI.HelpBox_Layout(message, MessageType.Warning,
                    context: property.serializedObject.targetObject);
                EditorGUILayout.PropertyField(property, true);
            }
        }

        public void ClearCache()
        {
            _reorderableListsByPropertyName.Clear();
        }

        private Object GetAssignableObject(Object obj, ReorderableList list)
        {
            System.Type listType = PropertyUtility.GetPropertyType(list.serializedProperty);
            System.Type elementType = ReflectionUtility.GetListElementType(listType);

            if (elementType == null)
            {
                return null;
            }

            System.Type objType = obj.GetType();

            if (elementType.IsAssignableFrom(objType))
            {
                return obj;
            }

            if (objType == typeof(GameObject))
            {
                if (typeof(Transform).IsAssignableFrom(elementType))
                {
                    Transform transform = ((GameObject)obj).transform;
                    if (elementType == typeof(RectTransform))
                    {
                        RectTransform rectTransform = transform as RectTransform;
                        return rectTransform;
                    }
                    else
                    {
                        return transform;
                    }
                }
                else if (typeof(MonoBehaviour).IsAssignableFrom(elementType))
                {
                    return ((GameObject)obj).GetComponent(elementType);
                }
            }

            return null;
        }
    }
}