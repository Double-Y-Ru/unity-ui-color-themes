using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Drawers
{
    // Draws color fields in a line, instead of a column
    [CustomPropertyDrawer(typeof(ThemeAlphaColorBlock))]
    public class ThemeAlphaColorBlockDrawer : PropertyDrawer
    {
        private SerializedProperty _normalColorProperty;
        private SerializedProperty _highlightedColorProperty;
        private SerializedProperty _pressedColorProperty;
        private SerializedProperty _selectedColorProperty;
        private SerializedProperty _disabledColorProperty;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2f * EditorGUIUtility.singleLineHeight;
        }

        private void Init(SerializedProperty property)
        {
            if (_normalColorProperty == null) _normalColorProperty = property.FindPropertyRelative("Normal");
            if (_highlightedColorProperty == null) _highlightedColorProperty = property.FindPropertyRelative("Highlighted");
            if (_pressedColorProperty == null) _pressedColorProperty = property.FindPropertyRelative("Pressed");
            if (_selectedColorProperty == null) _selectedColorProperty = property.FindPropertyRelative("Selected");
            if (_disabledColorProperty == null) _disabledColorProperty = property.FindPropertyRelative("Disabled");
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            Init(prop);

            // EditorGUI.BeginProperty(position, label, prop);
            Rect colorsTableRect = EditorGUI.PrefixLabel(position, label);

            //Rect colorsTableRect = EditorGUILayout.GetControlRect(hasLabel: true, 2f * EditorGUIUtility.singleLineHeight);
            float cellWidth = colorsTableRect.width / 5;

            Rect cellRect = new Rect(colorsTableRect.x, colorsTableRect.y, cellWidth - 2, colorsTableRect.height);

            ColorCell(cellRect, "Normal", _normalColorProperty);
            cellRect.x += cellWidth;

            ColorCell(cellRect, "Highlighted", _highlightedColorProperty);
            cellRect.x += cellWidth;

            ColorCell(cellRect, "Pressed", _pressedColorProperty);
            cellRect.x += cellWidth;

            ColorCell(cellRect, "Selected", _selectedColorProperty);
            cellRect.x += cellWidth;

            ColorCell(cellRect, "Disabled", _disabledColorProperty);
            cellRect.x += cellWidth;

            //EditorGUI.EndProperty();
        }

        private void ColorCell(Rect cellRect, string label, SerializedProperty colorProperty)
        {
            Rect headerCellRect = new Rect(cellRect.x, cellRect.y, cellRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(headerCellRect, label);

            Rect colorCellRect = new Rect(cellRect.x, headerCellRect.yMax, cellRect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginProperty(cellRect, new GUIContent(label), colorProperty);
            ThemesGUIUtility.ThemeAlphaColorProperty(colorCellRect, colorProperty, ThemesSettings.CurrentTheme);
            EditorGUI.EndProperty();
        }
    }
}
