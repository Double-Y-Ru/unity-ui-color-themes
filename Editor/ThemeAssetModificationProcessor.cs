using System;
using System.IO;
using UnityEditor;

namespace DoubleY.ColorThemes.Editor
{
    public class ThemeAssetModificationProcessor : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions _)
        {
            if (Path.GetExtension(assetPath) == (".asset"))
            {
                Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (typeof(ThemesSettings).IsAssignableFrom(assetType))
                {
                    ThemesSettingsEditor.DeleteSettingsAndKeyTable(ThemesSettings.Instance);
                }
            }
            return AssetDeleteResult.DidNotDelete;
        }
    }
}