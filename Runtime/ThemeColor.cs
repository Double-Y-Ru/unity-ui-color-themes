using System;
using UnityEngine;

namespace DoubleY.ColorThemes
{
    public readonly struct ThemeColor : IEquatable<ThemeColor>
    {
        public readonly long Id;
        public readonly string Name;
        public readonly Color Color;

        public ThemeColor(long id, string name, Color color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public static readonly ThemeColor Bad = new ThemeColor(-0, "Bad", ColorUtil.BadColor);

        public bool Equals(ThemeColor other)
        {
            return other.Id == Id
                && other.Name == Name
                && other.Color == Color;
        }
    }
}