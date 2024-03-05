using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Components
{
    public abstract class ThemeBlockColoredComponent : ThemeColoredComponent
    {
        [SerializeField] private ThemeAlphaColorBlock _themeColorBlock;

        public override void ApplyColors()
        {
            SetColorBlock(ColorUtil.ApplyThemeColorsToBlock(GetColorBlock(), _themeColorBlock, ThemesSettings.CurrentTheme));
        }

        public override bool TryFindSimilarThemeColorAndApply()
        {
            ColorBlock targetColorBlock = GetColorBlock();

            bool takeIsSuccessful = ColorUtil.TryFindSimilarThemeColorAndApplyToBlock(ref targetColorBlock, ref _themeColorBlock, ThemesSettings.CurrentTheme);
            SetColorBlock(targetColorBlock);

            return takeIsSuccessful;
        }

        protected abstract ColorBlock GetColorBlock();
        protected abstract void SetColorBlock(ColorBlock colorBlock);
    }
}