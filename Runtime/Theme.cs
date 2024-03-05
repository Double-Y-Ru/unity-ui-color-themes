#nullable enable

using System.Linq;
using UnityEngine;

namespace DoubleY.ColorThemes
{
    public class Theme
    {
        private readonly ThemesSettings? _settings;

        public Theme(ThemesSettings? settings)
        {
            _settings = settings;
        }

        public ThemeColor[] GetAllColors()
        {
            if (_settings == null) return new ThemeColor[0];

            return _settings
                .GetColorIds()
                .Select(colorId => new ThemeColor(
                    colorId,
                    _settings.GetColorName(colorId),
                    _settings.GetTableColor(_settings.CurrentThemeId, colorId)))
                .ToArray();
        }

        public ThemeColor GetColor(long colorId)
        {
            if (_settings == null) return ThemeColor.Bad;
            return new ThemeColor(colorId, _settings.GetColorName(colorId), _settings.GetTableColor(_settings.CurrentThemeId, colorId));
        }

        public Color GetColorValue(long colorId)
        {
            if (_settings == null) return ColorUtil.BadColor;
            return _settings.GetTableColor(_settings.CurrentThemeId, colorId);
        }
    }
}