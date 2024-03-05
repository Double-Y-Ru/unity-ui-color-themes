using DoubleY.ColorThemes.Components;
using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Components
{
    [CustomEditor(typeof(ThemeColoredComponent), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class ThemeColoredComponentEditor : UnityEditor.Editor
    {
        private bool _takeWasSuccessful = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            {
                GUIContent applyButtonContent = EditorGUIUtility.TrTextContent("Apply", "Apply chosen theme color to the target.");
                if (GUILayout.Button(applyButtonContent, EditorStyles.miniButton))
                    foreach (ThemeColoredComponent colorCustomizer in targets)
                        ThemeColoredComponentUtility.ApplyColors(colorCustomizer);

                Color prevColor = GUI.color;
                if (!_takeWasSuccessful)
                    GUI.color = Color.red;

                GUIContent findAndApplyButtonContent = EditorGUIUtility.TrTextContent(
                    "Try find similar and Apply" + (_takeWasSuccessful ? string.Empty : " (falied)"),
                    "Find theme color similar to the target's one. On success, apply it on both this component and target.");

                if (GUILayout.Button(findAndApplyButtonContent, EditorStyles.miniButton))
                    foreach (ThemeColoredComponent colorCustomizer in targets)
                        _takeWasSuccessful &= ThemeColoredComponentUtility.TryFindSimilarThemeColorAndApply(colorCustomizer);

                GUI.color = prevColor;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
