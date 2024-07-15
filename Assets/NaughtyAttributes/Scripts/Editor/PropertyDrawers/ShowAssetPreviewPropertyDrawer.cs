using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowAssetPreviewAttribute))]
    public class ShowAssetPreviewPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Texture2D previewTexture = GetAssetPreview(property);
                if (previewTexture != null)
                {
                    return GetPropertyHeight(property) + GetAssetPreviewSize(property).y;
                }
                else
                {
                    return GetPropertyHeight(property);
                }
            }
            else
            {
                return GetPropertyHeight(property) + GetHelpBoxHeight();
            }
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Rect propertyRect = new Rect()
                {
                    x = rect.x,
                    y = rect.y,
                    width = rect.width,
                    height = EditorGUIUtility.singleLineHeight
                };

                EditorGUI.PropertyField(propertyRect, property, label);

                Texture2D previewTexture = GetAssetPreview(property);
                if (previewTexture != null)
                {
                    var assetPreviewSize = GetAssetPreviewSize(property);
                    var padding = new Vector2(6f, 6f);
                    Rect textureRect = new Rect()
                    {
                        x = rect.width - assetPreviewSize.x + padding.x / 2f + 20f,
                        y = rect.y + EditorGUIUtility.singleLineHeight + padding.y / 2f,
                        width = assetPreviewSize.x - padding.x,
                        height = assetPreviewSize.y - padding.y
                    };

                    Rect textureBGRect = new Rect()
                    {
                        x = rect.width - assetPreviewSize.x + 20f,
                        y = rect.y + EditorGUIUtility.singleLineHeight,
                        width = assetPreviewSize.x,
                        height = assetPreviewSize.y
                    };

                    EditorGUI.DrawRect(textureBGRect, new Color(0f, 0f, 0f, 0.5f));
                    GUI.DrawTexture(textureRect, previewTexture, ScaleMode.ScaleToFit);

                    var style = new GUIStyle(EditorStyles.miniLabel);
                    EditorGUI.LabelField(new Rect()
                    {
                        x = textureBGRect.x - 140f,
                        y = rect.y + EditorGUIUtility.singleLineHeight,
                        width = rect.width / 2f,
                        height = EditorGUIUtility.singleLineHeight,
                    }, "Format:" + previewTexture.format.ToString(), style);

                    EditorGUI.LabelField(new Rect()
                    {
                        x = textureBGRect.x - 140f,
                        y = rect.y + 2f * EditorGUIUtility.singleLineHeight,
                        width = rect.width / 2f,
                        height = EditorGUIUtility.singleLineHeight,
                    }, "Graphics:" + previewTexture.graphicsFormat.ToString(), style);

                    EditorGUI.LabelField(new Rect()
                    {
                        x = textureBGRect.x - 140f,
                        y = rect.y + 3f * EditorGUIUtility.singleLineHeight,
                        width = rect.width / 2f,
                        height = EditorGUIUtility.singleLineHeight,
                    }, "Size:" + previewTexture.width + "x" + previewTexture.height, style);
                }
            }
            else
            {
                string message = property.name + " doesn't have an asset preview";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        private Texture2D GetAssetPreview(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                    return previewTexture;
                }

                return null;
            }

            return null;
        }

        private Vector2 GetAssetPreviewSize(SerializedProperty property)
        {
            Texture2D previewTexture = GetAssetPreview(property);
            if (previewTexture == null)
            {
                return Vector2.zero;
            }
            else
            {
                int targetWidth = ShowAssetPreviewAttribute.DefaultWidth;
                int targetHeight = ShowAssetPreviewAttribute.DefaultHeight;

                ShowAssetPreviewAttribute showAssetPreviewAttribute =
                    PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);
                if (showAssetPreviewAttribute != null)
                {
                    targetWidth = showAssetPreviewAttribute.Width;
                    targetHeight = showAssetPreviewAttribute.Height;
                }

                int width = Mathf.Clamp(targetWidth, 100, previewTexture.width);
                int height = Mathf.Clamp(targetHeight, 100, previewTexture.height);

                return new Vector2(width, height) / 2f;
            }
        }
    }
}