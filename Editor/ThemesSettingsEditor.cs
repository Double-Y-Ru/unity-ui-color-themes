using UnityEditor;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor
{
    public static class ThemesSettingsEditor
    {
        public static ThemesSettings CreateSettingsAndKeyTable(string settingsPath)
        {
            ThemesSettings settings = ScriptableObject.CreateInstance<ThemesSettings>();
            settings.name = "Themes Settings";
            AssetDatabase.CreateAsset(settings, settingsPath);
            Undo.RegisterCreatedObjectUndo(settings, "Add settings");

            EditorBuildSettings.AddConfigObject(ThemesSettings.ConfigName, settings, overwrite: true);
            return settings;
        }

        public static void DeleteSettingsAndKeyTable(ThemesSettings settings)
        {
            Undo.RecordObject(settings, "Remove settings");

            foreach (string themeId in settings.GetThemeIds())
                DeleteTheme(settings, themeId);

            foreach (long colorId in settings.GetColorIds())
                DeleteColor(settings, colorId);

            Undo.DestroyObjectImmediate(settings);
            EditorBuildSettings.RemoveConfigObject(ThemesSettings.ConfigName);
        }

        public static void AddColor(ThemesSettings settings, string colorName)
        {
            Undo.RecordObject(settings, "Add color");
            settings.AddColor(colorName);
            EditorUtility.SetDirty(settings);
        }

        public static void DeleteColor(ThemesSettings settings, long colorId)
        {
            Undo.RecordObject(settings, "Delete color");
            settings.DeleteColor(colorId);
            EditorUtility.SetDirty(settings);
        }

        public static void SetColorName(ThemesSettings settings, long themeId, string colorName)
        {
            Undo.RecordObject(settings, "Set color name");
            settings.SetColorName(themeId, colorName);
            EditorUtility.SetDirty(settings);
        }

        public static void SetColor(ThemesSettings settings, string themeId, long colorId, Color color)
        {
            Undo.RecordObject(settings, "Set color");
            settings.SetTableColor(themeId, colorId, color);
            EditorUtility.SetDirty(settings);
        }

        public static void SetCurrentThemeId(ThemesSettings settings, string themeId)
        {
            Undo.RecordObject(settings, "Set current theme");
            settings.CurrentThemeId = themeId;
            EditorUtility.SetDirty(settings);
        }

        public static void AddTheme(ThemesSettings settings, string themeId)
        {
            Undo.RecordObject(settings, "Add theme");
            settings.AddTheme(themeId);
            EditorUtility.SetDirty(settings);
        }

        public static void DeleteTheme(ThemesSettings settings, string themeId)
        {
            Undo.RecordObject(settings, "Remove theme");
            settings.RemoveTheme(themeId);
            EditorUtility.SetDirty(settings);
        }

        public static void RenameTheme(ThemesSettings settings, string themeId, string newThemeId)
        {
            Undo.RecordObject(settings, "Remove theme");
            settings.RenameTheme(themeId, newThemeId);
            EditorUtility.SetDirty(settings);
        }

        public static void SaveAll(ThemesSettings settings)
        {
            AssetDatabase.SaveAssetIfDirty(settings);
        }
    }
}