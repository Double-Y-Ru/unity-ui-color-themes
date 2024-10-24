using DoubleY.ColorThemes.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Editor.Components
{
    public static class ColorThemeActions
    {
        private const string UseColorThemeForGraphicMenuName = "CONTEXT/Graphic/" + ThemeColoredComponentUtility.UseColorThemeMenuName + " (Graphic)";
        private const string UseColorThemeForSelectableMenuName = "CONTEXT/Selectable/" + ThemeColoredComponentUtility.UseColorThemeMenuName + " (Selectable)";
        private const string UseColorThemeForShadowMenuName = "CONTEXT/Shadow/" + ThemeColoredComponentUtility.UseColorThemeMenuName + " (Shadow)";

        [MenuItem(UseColorThemeForGraphicMenuName)] private static void UseColorThemeForGraphic(MenuCommand command) => ThemeColoredComponentUtility.RequireThemeColorerAndTryFindSimilarThemeColorAndApply<ThemeColoredGraphic>(command.context as Component);
        [MenuItem(UseColorThemeForSelectableMenuName)] private static void UseColorThemeForSelectable(MenuCommand command) => ThemeColoredComponentUtility.RequireThemeColorerAndTryFindSimilarThemeColorAndApply<ThemeColoredSelectable>(command.context as Component);
        [MenuItem(UseColorThemeForShadowMenuName)] private static void UseColorThemeForShadow(MenuCommand command) => ThemeColoredComponentUtility.RequireThemeColorerAndTryFindSimilarThemeColorAndApply<ThemeColoredShadow>(command.context as Component);

        [MenuItem(UseColorThemeForGraphicMenuName, validate = true)] private static bool ValidateUseColorThemeForGraphic(MenuCommand command) => command.context is Graphic;
        [MenuItem(UseColorThemeForSelectableMenuName, validate = true)] private static bool ValidateUseColorThemeForSelectable(MenuCommand command) => command.context is Selectable;
        [MenuItem(UseColorThemeForShadowMenuName, validate = true)] private static bool ValidateUseColorThemeForShadow(MenuCommand command) => command.context is Shadow;
    }
}
