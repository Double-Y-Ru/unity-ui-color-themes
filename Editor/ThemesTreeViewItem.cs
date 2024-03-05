using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor
{
    public class ThemesTreeViewItem : TreeViewItem
    {
        public readonly ThemesSettings ThemesSettings;
        public readonly long ThemeColorId;

        public override int id
        {
            get => (int)ThemeColorId;
            set => throw new InvalidOperationException();
        }

        public override string displayName
        {
            get => ColorName;
            set => ColorName = value;
        }

        public string ColorName
        {
            get => ThemesSettings.GetColorName(ThemeColorId);
            set => ThemesSettings.SetColorName(ThemeColorId, value);
        }

        public ThemesTreeViewItem(ThemesSettings themeSettings, long themeColorId)
        {
            ThemesSettings = themeSettings;
            ThemeColorId = themeColorId;
        }

        public Color GetColor(string themeId)
        {
            return ThemesSettings.GetTableColor(themeId, ThemeColorId);
        }
    }
}