using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ThemeColor))]
    public class ThemeColorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect valuesRect = EditorGUI.PrefixLabel(position, label);
            long themeColorId = property.longValue;

            EditorGUI.BeginChangeCheck();
            long newThemeColorId = ThemesGUIUtility.ThemeColorField(valuesRect, ThemesSettings.CurrentTheme, themeColorId);
            if (EditorGUI.EndChangeCheck())
                property.longValue = newThemeColorId;

            EditorGUI.EndProperty();
        }
    }
}
