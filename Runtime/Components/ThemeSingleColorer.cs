using UnityEngine;

namespace DoubleY.ColorThemes.Components
{
    public abstract class ThemeSingleColorer : ThemeColorer
    {
        [SerializeField]
        private ThemeAlphaColor _themeColor;

        public override void ApplyColors()
        {
            SetColor(_themeColor.GetColor(ThemesSettings.CurrentTheme));
        }

        public override bool TryFindSimilarThemeColorAndApply()
        {
            if (ColorUtil.TryFindSimilarThemeColor(GetColor(), ThemesSettings.CurrentTheme, out ThemeAlphaColor foundThemeAlphaColor))
            {
                _themeColor = foundThemeAlphaColor;
                ApplyColors();
                return true;
            }
            return false;
        }

        protected abstract Color GetColor();
        protected abstract void SetColor(Color color);
    }
}