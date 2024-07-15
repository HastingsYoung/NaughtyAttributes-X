using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ExpandableInfoBoxAttribute))]
    public class ExpandableInfoBoxDecoratorDrawer : DecoratorDrawer
    {
        private bool isExpanded;

        public override float GetHeight()
        {
            return GetHeaderHeight() + (isExpanded ? GetExpandableHeight() : 0f);
        }

        public override void OnGUI(Rect rect)
        {
            ExpandableInfoBoxAttribute infoBoxAttribute = (ExpandableInfoBoxAttribute)attribute;

            float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
            Rect headerRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHeaderHeight());
            Rect contentRect = new Rect(rect.x + indentLength,
                rect.y + headerRect.height,
                rect.width - indentLength,
                GetExpandableHeight());

            DrawInfoBox(headerRect, contentRect, infoBoxAttribute.ShortText, infoBoxAttribute.LongText,
                infoBoxAttribute.Type);
        }

        private float GetHeaderHeight()
        {
            ExpandableInfoBoxAttribute infoBoxAttribute = (ExpandableInfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 1.0f;

            float desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.ShortText),
                EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, desiredHeight);

            return height;
        }

        private float GetExpandableHeight()
        {
            ExpandableInfoBoxAttribute infoBoxAttribute = (ExpandableInfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 1.0f;

            float expandedHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.LongText),
                EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, expandedHeight);

            return height;
        }

        private void DrawInfoBox(Rect rect, Rect contentRect, string shortTxt, string longTxt, EInfoBoxType infoBoxType)
        {
            MessageType messageType = MessageType.None;
            switch (infoBoxType)
            {
                case EInfoBoxType.Normal:
                    messageType = MessageType.Info;
                    break;

                case EInfoBoxType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case EInfoBoxType.Error:
                    messageType = MessageType.Error;
                    break;
            }

            EditorGUILayout.BeginVertical();
            EditorGUI.HelpBox(rect, shortTxt, messageType);
            isExpanded = EditorGUI.Foldout(rect, isExpanded, new GUIContent());
            if (isExpanded)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextArea(contentRect, longTxt, EditorStyles.textArea);
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}