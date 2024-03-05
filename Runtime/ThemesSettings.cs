using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoubleY.ColorThemes
{
    public class ThemesSettings : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        private class KeyTableColor
        {
            [SerializeField] public long Id;
            [SerializeField] public string Name;

            public KeyTableColor(long id, string name) { Id = id; Name = name; }
        }

        [Serializable]
        private class ThemeTableColor
        {
            [SerializeField] public long Id;
            [SerializeField] public Color Color;

            public ThemeTableColor(long id, Color color) { Id = id; Color = color; }
        }

        [Serializable]
        private class ThemeTable
        {
            [SerializeField] public string ThemeId = "Theme";
            [SerializeField] public ThemeTableColor[] Colors = new ThemeTableColor[0];
        }

        public const string ConfigName = "com.double-y.color-themes.settings";

        private static ThemesSettings _instance;
        private static readonly Theme EmptyTheme = new Theme(null);

        [SerializeField] private DistributedUIDGenerator _keyGen = new DistributedUIDGenerator();
        [SerializeField] private string _currentThemeId = string.Empty;
        [SerializeField] private KeyTableColor[] _keysTable = new KeyTableColor[0];
        [SerializeField] private ThemeTable[] _themeTables = new ThemeTable[0];

        private Theme _currentTheme;
        private Dictionary<string, Dictionary<long, Color>> _themes = new Dictionary<string, Dictionary<long, Color>>();
        private Dictionary<long, string> _keys = new Dictionary<long, string>();

        public static ThemesSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GetInstanceDontCreateDefault();

                return _instance;
            }
            set => _instance = value;
        }

        public static Theme CurrentTheme
        {
            get
            {
                if (Instance == null)
                {
                    Debug.LogWarning("Theme settings are missing");
                    return EmptyTheme;
                }
                else
                    return Instance.CurrentThemeInternal;
            }
        }

        public string CurrentThemeId
        {
            get => _currentThemeId;
            set
            {
                if (!GetThemeIds().Contains(value))
                    throw new InvalidOperationException($"Theme with id {value} does not exist");

                _currentThemeId = value;
            }
        }

        internal Theme CurrentThemeInternal
        {
            get
            {
                if (_currentTheme == null)
                    _currentTheme = new Theme(this);

                return _currentTheme;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public void OnAfterDeserialize()
        {
            _themes = new Dictionary<string, Dictionary<long, Color>>();
            foreach (ThemeTable themeTable in _themeTables)
            {
                Dictionary<long, Color> themeMap = new Dictionary<long, Color>();
                foreach (ThemeTableColor themeTableColor in themeTable.Colors)
                    themeMap[themeTableColor.Id] = themeTableColor.Color;

                _themes[themeTable.ThemeId] = themeMap;
            }

            _keys = new Dictionary<long, string>();
            foreach (KeyTableColor keyTableColor in _keysTable)
                _keys[keyTableColor.Id] = keyTableColor.Name;
        }

        public void OnBeforeSerialize()
        {
            _themeTables = _themes
                .Select(themeMapPair => new ThemeTable()
                {
                    ThemeId = themeMapPair.Key,
                    Colors = themeMapPair.Value
                                .Select(themeColorPair => new ThemeTableColor(themeColorPair.Key, themeColorPair.Value))
                                .OrderBy(color => color.Id)
                                .ToArray()
                })
                .ToArray();

            _keysTable = _keys
                .Select(kvp => new KeyTableColor(kvp.Key, kvp.Value))
                .OrderBy(color => color.Id)
                .ToArray();
        }

        public long AddColor(string colorName)
        {
            long newColorId = _keyGen.GetNextKey();

            _keys[newColorId] = colorName;
            foreach (Dictionary<long, Color> theme in _themes.Values)
                theme[newColorId] = ColorUtil.BadColor;

            return newColorId;
        }

        public void DeleteColor(long colorId)
        {
            foreach (Dictionary<long, Color> theme in _themes.Values)
                theme.Remove(colorId);
            _keys.Remove(colorId);
        }

        public IEnumerable<long> GetColorIds() => _keys.Keys.OrderBy(k => k);
        public IEnumerable<string> GetThemeIds() => _themes.Keys.OrderBy(k => k);
        public IEnumerable<Color> GetThemeColors(string themeId) => _themes[themeId].OrderBy(pair => pair.Key).Select(pair => pair.Value);

        public Color GetTableColor(string themeId, long colorId) => _themes[themeId][colorId];
        public void SetTableColor(string themeId, long colorId, Color color) => _themes[themeId][colorId] = color;

        public string GetColorName(long colorId) => _keys[colorId];
        public void SetColorName(long colorId, string colorName) => _keys[colorId] = colorName;

        public void AddTheme(string themeId)
        {
            Dictionary<long, Color> themeColors = new Dictionary<long, Color>();
            foreach (long colorId in _keys.Keys)
                themeColors[colorId] = ColorUtil.BadColor;

            _themes.Add(themeId, themeColors);
            if (_themes.Count == 1)
                CurrentThemeId = themeId;
        }

        public bool RemoveTheme(string themeId)
        {
            bool removed = _themes.Remove(themeId);

            if (_themes.Count == 0)
                _currentThemeId = string.Empty;
            else if (themeId == _currentThemeId)
                CurrentThemeId = GetThemeIds().First();

            return removed;
        }

        public void RenameTheme(string themeId, string newThemeId)
        {
            Dictionary<long, Color> theme = _themes[themeId];

            RemoveTheme(themeId);
            AddTheme(newThemeId);

            foreach (KeyValuePair<long, Color> kvp in theme)
                SetTableColor(newThemeId, kvp.Key, kvp.Value);
        }

        private static ThemesSettings GetInstanceDontCreateDefault()
        {
            if (_instance != null)
                return _instance;

            ThemesSettings settings;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject(ConfigName, out settings);
#else
            settings = FindObjectOfType<ThemesSettings>();
#endif
            return settings;
        }
    }
}
