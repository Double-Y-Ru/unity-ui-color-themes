using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ThemeAlphaColor))]
    public class ThemeAlphaColorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect valuesRect = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginProperty(position, label, property);
            ThemesGUIUtility.ThemeAlphaColorProperty(valuesRect, property, ThemesSettings.CurrentTheme);
            EditorGUI.EndProperty();
        }
    }
}
