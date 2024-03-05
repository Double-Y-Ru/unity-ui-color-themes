using DoubleY.ColorThemes.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Components
{
    public static class ThemeColoredComponentUtility
    {
        public static void CheckAndAddUseThemeButton<TComponentThemeColorer>(IEnumerable<Component> targetComponents) where TComponentThemeColorer : ThemeColoredComponent
        {
            if (GUILayout.Button("Use Color Theme", EditorStyles.miniButton))
            {
                bool someTargetsHaveConfiguringComponents = targetComponents.Any(targetComponent => targetComponent.TryGetComponent<TComponentThemeColorer>(out _));
                if (someTargetsHaveConfiguringComponents) return;

                foreach (Component targetComponent in targetComponents)
                {
                    TComponentThemeColorer addedThemeColorer = Undo.AddComponent<TComponentThemeColorer>(targetComponent.gameObject);
                    TryFindSimilarThemeColorAndApply(addedThemeColorer);
                }
            }
        }

        public static void ApplyColors(ThemeColoredComponent themeColorer)
        {
            Undo.RecordObject(themeColorer.GetTargetComponent(), "Set Color");
            themeColorer.ApplyColors();
        }

        public static bool TryFindSimilarThemeColorAndApply(ThemeColoredComponent themeColorer)
        {
            if (themeColorer.TryFindSimilarThemeColorAndApply())
            {
                EditorUtility.SetDirty(themeColorer);
                return true;
            }

            return false;
        }
    }
}
