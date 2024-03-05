using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DoubleY.ColorThemes
{
    public class ColorThemesBuildPlayer : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private ThemesSettings _settings;

        private bool _removeFromPreloadedAssets;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            _removeFromPreloadedAssets = false;
            _settings = ThemesSettings.Instance;
            if (_settings == null)
                return;

            // Add the localization settings to the preloaded assets.
            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            bool wasDirty = IsPlayerSettingsDirty();

            if (!preloadedAssets.Contains(_settings))
            {
                ArrayUtility.Add(ref preloadedAssets, _settings);
                PlayerSettings.SetPreloadedAssets(preloadedAssets);

                // If we have to add the settings then we should also remove them.
                _removeFromPreloadedAssets = true;

                // Clear the dirty flag so we dont flush the modified file (case 1254502)
                if (!wasDirty)
                    ClearPlayerSettingsDirtyFlag();
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (_settings == null || !_removeFromPreloadedAssets)
                return;

            bool wasDirty = IsPlayerSettingsDirty();

            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            ArrayUtility.Remove(ref preloadedAssets, _settings);
            PlayerSettings.SetPreloadedAssets(preloadedAssets);

            _settings = null;

            // Clear the dirty flag so we dont flush the modified file (case 1254502)
            if (!wasDirty)
                ClearPlayerSettingsDirtyFlag();
        }

        private static bool IsPlayerSettingsDirty()
        {
            PlayerSettings[] settings = Resources.FindObjectsOfTypeAll<PlayerSettings>();
            if (settings != null && settings.Length > 0)
                return EditorUtility.IsDirty(settings[0]);
            return false;
        }

        private static void ClearPlayerSettingsDirtyFlag()
        {
            PlayerSettings[] settings = Resources.FindObjectsOfTypeAll<PlayerSettings>();
            if (settings != null && settings.Length > 0)
                EditorUtility.ClearDirty(settings[0]);
        }
    }
}
