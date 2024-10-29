using DoubleY.ColorThemes.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Editor.Components
{
    public static class ThemeColorerActions
    {
        public const string UseColorThemeMenuName = "Use Color Theme";

        private const string UseColorThemeForGraphicMenuName = "CONTEXT/" + nameof(Graphic) + "/" + UseColorThemeMenuName + " (" + nameof(Graphic) + ")";
        private const string UseColorThemeForSelectableMenuName = "CONTEXT/" + nameof(Selectable) + "/" + UseColorThemeMenuName + " (" + nameof(Selectable) + ")";
        private const string UseColorThemeForShadowMenuName = "CONTEXT/" + nameof(Shadow) + "/" + UseColorThemeMenuName + " (" + nameof(Shadow) + ")";

        [MenuItem(UseColorThemeForGraphicMenuName)] private static void UseColorThemeForGraphic(MenuCommand command) => ThemeColorerUtility.RequireThemeColorerAndTryFindSimilarThemeColorAndApply<GraphicThemeColorer>(command.context as Component);
        [MenuItem(UseColorThemeForSelectableMenuName)] private static void UseColorThemeForSelectable(MenuCommand command) => ThemeColorerUtility.RequireThemeColorerAndTryFindSimilarThemeColorAndApply<SelectableThemeColorer>(command.context as Component);
        [MenuItem(UseColorThemeForShadowMenuName)] private static void UseColorThemeForShadow(MenuCommand command) => ThemeColorerUtility.RequireThemeColorerAndTryFindSimilarThemeColorAndApply<ShadowThemeColorer>(command.context as Component);

        [MenuItem(UseColorThemeForGraphicMenuName, validate = true)] private static bool ValidateUseColorThemeForGraphic(MenuCommand command) => command.context is Graphic;
        [MenuItem(UseColorThemeForSelectableMenuName, validate = true)] private static bool ValidateUseColorThemeForSelectable(MenuCommand command) => command.context is Selectable;
        [MenuItem(UseColorThemeForShadowMenuName, validate = true)] private static bool ValidateUseColorThemeForShadow(MenuCommand command) => command.context is Shadow;
    }
}
