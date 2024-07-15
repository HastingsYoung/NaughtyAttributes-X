namespace NaughtyAttributes.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(IndicatorAttribute))]
    public class IndicatorPropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attribute = PropertyUtility.GetAttribute<IndicatorAttribute>(property);
            var bgOffset = new Vector2(0f, 1f);
            var bgSize = new Vector2(rect.height - 2f, rect.height - 2f);
            var hlOffset = new Vector2(2f, 2.5f);
            var hlSize = new Vector2(rect.height - 6f, rect.height - 6f);
            var textSize = new Vector2(rect.width - bgSize.x - 4f, rect.height);
            EditorGUI.LabelField(new Rect(rect.x, rect.y, textSize.x, textSize.y), property.displayName);
            EditorGUI.DrawRect(
                new Rect(rect.x + textSize.x + bgOffset.x, rect.y + bgOffset.y, bgSize.x, bgSize.y),
                EColor.Black.GetColor());
            EditorGUI.DrawRect(
                new Rect(rect.x + textSize.x + hlOffset.x, rect.y + hlOffset.y, hlSize.x, hlSize.y),
                property.boolValue ? attribute.TrueColor.GetColor() : attribute.FalseColor.GetColor());
        }
    }
}