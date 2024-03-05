using System;

namespace DoubleY.ColorThemes
{
    [Serializable]
    public struct ThemeAlphaColorBlock
    {
        public ThemeAlphaColor Normal;
        public ThemeAlphaColor Highlighted;
        public ThemeAlphaColor Pressed;
        public ThemeAlphaColor Selected;
        public ThemeAlphaColor Disabled;
    }
}