using DoubleY.ColorThemes.Components;
using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor.Components
{
    public static class ThemeColoredComponentUtility
    {
        public const string UseColorThemeMenuName = "Use Color Theme";

        public static void RequireThemeColorerAndTryFindSimilarThemeColorAndApply<TComponentThemeColorer>(Component targetComponent) where TComponentThemeColorer : ThemeColoredComponent
        {
            TComponentThemeColorer addedThemeColorer = RequireThemeColorer<TComponentThemeColorer>(targetComponent);
            TryFindSimilarThemeColorAndApply(addedThemeColorer);
        }

        public static TComponentThemeColorer RequireThemeColorer<TComponentThemeColorer>(Component targetComponent) where TComponentThemeColorer : ThemeColoredComponent
        {
            return targetComponent.TryGetComponent(out TComponentThemeColorer themeColorer)
                ? themeColorer
                : Undo.AddComponent<TComponentThemeColorer>(targetComponent.gameObject);
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
