using System;
using UnityEngine;

namespace DoubleY.ColorThemes
{
    [Serializable]
    public struct ThemeAlphaColor : IEquatable<ThemeAlphaColor>
    {
        public long ThemeColorId;
        public float Opacity;

        public ThemeAlphaColor(long themeColorId) : this(themeColorId, 1f)
        { }

        public ThemeAlphaColor(long themeColorId, float opacity)
        {
            ThemeColorId = themeColorId;
            Opacity = opacity;
        }

        public Color GetColor(Theme theme)
        {
            Color color = theme.GetColor(ThemeColorId).Color;
            color.a = Opacity;
            return color;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != GetType()) return false;
            return Equals((ThemeAlphaColor)obj);
        }

        public bool Equals(ThemeAlphaColor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Mathf.Abs(Opacity - other.Opacity) < 0.01f
                && ThemeColorId == other.ThemeColorId;
        }

        public override int GetHashCode() => HashCode.Combine(ThemeColorId, Opacity);
    }
}