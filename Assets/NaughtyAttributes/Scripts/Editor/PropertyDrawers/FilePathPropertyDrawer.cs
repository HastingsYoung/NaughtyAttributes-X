namespace NaughtyAttributes.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(NaughtyAttributes.FilePathAttribute))]
    public class FilePathPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property);
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attribute = PropertyUtility.GetAttribute<NaughtyAttributes.FilePathAttribute>(property);
            EditorGUILayout.LabelField(property.displayName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(property.stringValue);
            if (GUILayout.Button(EditorGUIUtility.IconContent("Icon_Editor_OpenFile"), GUILayout.Width(60f),
                    GUILayout.Height(20f)))
            {
                var openPath = string.IsNullOrEmpty(property.stringValue)
                    ? attribute.DefaultParent
                    : property.stringValue;
                property.stringValue = EditorPathUtil.ConvertPathRelativeToAssets(
                    EditorUtility.OpenFilePanel("Select your saving path...", openPath, attribute.Extensions));
                property.serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}