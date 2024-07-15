namespace NaughtyAttributes.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            // Divide by 2f to remove empty spaces
            return GetPropertyHeight(property) / 2f;
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attribute = PropertyUtility.GetAttribute<FolderPathAttribute>(property);
            EditorGUILayout.LabelField(property.displayName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(property.stringValue);
            if (GUILayout.Button(EditorGUIUtility.IconContent("Icon_Editor_OpenFolder"), GUILayout.Width(60f),
                    GUILayout.Height(20f)))
            {
                var openPath = string.IsNullOrEmpty(property.stringValue)
                    ? attribute.DefaultPath
                    : property.stringValue;
                property.stringValue = EditorPathUtil.ConvertPathRelativeToAssets(
                    EditorUtility.OpenFolderPanel("Select your saving path...", openPath, ""));
                property.serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}