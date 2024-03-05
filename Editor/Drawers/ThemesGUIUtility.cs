using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Drawers
{
    public static class ThemesGUIUtility
    {
        private const float OpacityFieldWidthPx = 28f;
        private const float OpacityStripeMinHeightPx = 2f;
        private const float OpacityStripeMaxHeightPx = 20f;
        private const float OpacityStripeRelativeHeight = 0.2f;

        private static readonly GUIStyle ColorBarGuiStyle = new GUIStyle(EditorStyles.colorField)
        {
            fontStyle = FontStyle.Normal,
            padding = new RectOffset(3, 0, 0, 0), // Small indent for theme color name
            normal = new GUIStyleState()
            {
                scaledBackgrounds = EditorStyles.colorField.normal.scaledBackgrounds,
                background = EditorGUIUtility.whiteTexture,
                textColor = Color.black,
            }
        };

        public static void ThemeAlphaColorProperty(Rect position, SerializedProperty property, Theme theme)
        {
            SerializedProperty themeColorProperty = property.FindPropertyRelative("ThemeColorId");
            SerializedProperty opacityProperty = property.FindPropertyRelative("Opacity");

            // The stripe should be thinner than the color field, like in the default color watch
            float opacityStripeHeightPx = Mathf.Clamp(position.height * OpacityStripeRelativeHeight, OpacityStripeMinHeightPx, OpacityStripeMaxHeightPx);

            Rect colorFieldRect = new Rect(position.x, position.y, position.width - OpacityFieldWidthPx, position.height - opacityStripeHeightPx);
            Rect opacityFieldRect = new Rect(colorFieldRect.xMax, colorFieldRect.y, OpacityFieldWidthPx, position.height);
            Rect opacityStripeRect = new Rect(colorFieldRect.x, colorFieldRect.yMax, colorFieldRect.width, opacityStripeHeightPx);

            long themeColorId = themeColorProperty.longValue;

            EditorGUI.BeginChangeCheck();
            long newThemeColorId = ThemeColorField(colorFieldRect, theme, themeColorId);
            if (EditorGUI.EndChangeCheck())
                themeColorProperty.longValue = newThemeColorId;

            Color prevBackgroundColor = GUI.backgroundColor;
            Color backgroundColor = GUI.backgroundColor;
            backgroundColor.a = 0.5f;
            GUI.backgroundColor = backgroundColor;

            float opacityValue = opacityProperty.floatValue;
            int opacityPercent = Mathf.RoundToInt(opacityProperty.floatValue * 100f);

            EditorGUI.BeginChangeCheck();

            int prevIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            int newOpacityPercent = Mathf.Clamp(EditorGUI.IntField(opacityFieldRect, opacityPercent), 0, 100);
            if (EditorGUI.EndChangeCheck())
            {
                opacityValue = 0.01f * newOpacityPercent;
                opacityProperty.floatValue = opacityValue;
            }
            EditorGUI.indentLevel = prevIndent;

            GUI.backgroundColor = prevBackgroundColor;

            DrawStripe(opacityStripeRect, opacityValue);
        }

        public static long ThemeColorField(Rect position, Theme theme, long selectedThemeColorId)
        {
            ThemeColor[] allThemeColors = theme.GetAllColors();

            int selectedThemeColorIndex = Array.FindIndex(allThemeColors, c => c.Id == selectedThemeColorId);

            ThemeColor selectedColor = selectedThemeColorIndex == -1 ? ThemeColor.Bad : theme.GetColor(selectedThemeColorId);

            ColorBarGuiStyle.normal.textColor = selectedColor.Color.grayscale > 0.5f ? Color.black : Color.white;

            int prevIndent = EditorGUI.indentLevel;
            Color prevBgColor = GUI.backgroundColor;

            EditorGUI.indentLevel = 0;
            GUI.backgroundColor = selectedColor.Color;

            GUIContent[] allThemeColorContents = allThemeColors.Select(GetContentForThemeColor).ToArray();

            int newSelectedThemeColorIndex = EditorGUI.Popup(position, selectedThemeColorIndex, allThemeColorContents, ColorBarGuiStyle);

            GUI.backgroundColor = prevBgColor;
            EditorGUI.indentLevel = prevIndent;

            return newSelectedThemeColorIndex == -1
                ? ThemeColor.Bad.Id
                : allThemeColors[newSelectedThemeColorIndex].Id;
        }

        public static void DrawStripe(Rect position, float value01)
        {
            if (Event.current.type != EventType.Repaint) return;

            Color prevColor = GUI.color;
            float a = (GUI.enabled ? 1 : 2);
            GUIStyle stripeGuiStyle = new GUIStyle { normal = { background = EditorGUIUtility.whiteTexture } };

            GUI.color = new Color(0f, 0f, 0f, a);
            stripeGuiStyle.Draw(position, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);

            GUI.color = new Color(1f, 1f, 1f, a);
            position.width *= Mathf.Clamp01(value01);
            stripeGuiStyle.Draw(position, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);

            GUI.color = prevColor;
        }

        private static GUIContent GetContentForThemeColor(ThemeColor themeColor)
        {
            return new GUIContent(themeColor.Name);
        }
    }
}
