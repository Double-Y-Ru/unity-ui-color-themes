using System;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes
{
    public static class ColorUtil
    {
        public static readonly Color BadColor = Color.magenta;
        public const float ColorComparisonTolerance = 0.01f;

        public static bool EpsilonEquals(this Color color, Color other, float epsilon)
        {
            return Mathf.Abs(color.r - other.r) < epsilon
                && Mathf.Abs(color.g - other.g) < epsilon
                && Mathf.Abs(color.b - other.b) < epsilon
                && Mathf.Abs(color.a - other.a) < epsilon;
        }

        public static ColorBlock ApplyThemeColorsToBlock(ColorBlock colorBlock, ThemeAlphaColorBlock themeColorBlock, Theme theme)
        {
            for (int colorIndex = 0; colorIndex < 5; ++colorIndex)
                colorBlock.SetColor(colorIndex, themeColorBlock.GetColor(colorIndex).GetColor(theme));

            return colorBlock;
        }

        public static bool TryFindSimilarThemeColorAndApplyToBlock(ref ColorBlock colorBlock, ref ThemeAlphaColorBlock themeColorBlock, Theme theme)
        {
            bool takeIsSuccessful = true;
            for (int colorIndex = 0; colorIndex < 5; ++colorIndex)
                takeIsSuccessful &= TryFindSimilarThemeColorAndApplyToBlockColor(ref colorBlock, ref themeColorBlock, theme, colorIndex);

            return takeIsSuccessful;
        }

        public static bool TryFindSimilarThemeColorAndApplyToBlockColor(ref ColorBlock colorBlock, ref ThemeAlphaColorBlock themeColorBlock, Theme theme, int colorIndex)
        {
            if (TryFindSimilarThemeColor(colorBlock.GetColor(colorIndex), theme, out ThemeAlphaColor foundThemeAlphaColor))
            {
                themeColorBlock.SetColor(colorIndex, foundThemeAlphaColor);
                colorBlock.SetColor(colorIndex, foundThemeAlphaColor.GetColor(ThemesSettings.CurrentTheme));
                return true;
            }
            return false;
        }

        public static bool TryFindSimilarThemeColor(Color color, Theme theme, out ThemeAlphaColor themeAlphaColor)
        {
            float opacity = color.a;
            color.a = 1f;

            ThemeColor[] themeColors = theme.GetAllColors();
            ThemeColor themeColor = Array.Find(themeColors, themeColor => themeColor.Color.EpsilonEquals(color, ColorComparisonTolerance));

            themeAlphaColor = new ThemeAlphaColor(themeColor.Id, opacity);
            return !themeColor.Equals(default(ThemeColor));
        }

        public static Color GetColor(this ColorBlock colorBlock, int colorIndex)
        {
            switch (colorIndex)
            {
                case 0: return colorBlock.normalColor;
                case 1: return colorBlock.highlightedColor;
                case 2: return colorBlock.pressedColor;
                case 3: return colorBlock.selectedColor;
                case 4: return colorBlock.disabledColor;
                default: throw new NotSupportedException();
            }
        }

        public static void SetColor(this ref ColorBlock colorBlock, int colorIndex, Color color)
        {
            switch (colorIndex)
            {
                case 0: colorBlock.normalColor = color; break;
                case 1: colorBlock.highlightedColor = color; break;
                case 2: colorBlock.pressedColor = color; break;
                case 3: colorBlock.selectedColor = color; break;
                case 4: colorBlock.disabledColor = color; break;
                default: throw new NotSupportedException();
            }
        }

        public static ThemeAlphaColor GetColor(this ThemeAlphaColorBlock themeColorBlock, int colorIndex)
        {
            switch (colorIndex)
            {
                case 0: return themeColorBlock.Normal;
                case 1: return themeColorBlock.Highlighted;
                case 2: return themeColorBlock.Pressed;
                case 3: return themeColorBlock.Selected;
                case 4: return themeColorBlock.Disabled;
                default: throw new NotSupportedException();
            }
        }

        public static void SetColor(this ref ThemeAlphaColorBlock themeColorBlock, int colorIndex, ThemeAlphaColor color)
        {
            switch (colorIndex)
            {
                case 0: themeColorBlock.Normal = color; return;
                case 1: themeColorBlock.Highlighted = color; return;
                case 2: themeColorBlock.Pressed = color; return;
                case 3: themeColorBlock.Selected = color; return;
                case 4: themeColorBlock.Disabled = color; return;
                default: throw new NotSupportedException();
            }
        }
    }
}
