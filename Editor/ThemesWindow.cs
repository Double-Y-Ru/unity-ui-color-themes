using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor
{
    public class ThemesWindow : EditorWindow
    {
        [SerializeField] private TreeViewState _treeViewState;
        [SerializeField] private MultiColumnHeaderState _multiColumnHeaderState;

        private ThemesTreeView _themesTreeView;
        private GUIContent _iconToolbarPlus;
        private GUIContent _iconToolbarMinus;
        private GUIContent _iconAddNew;
        private GUIContent _iconRename;
        private GUIContent _iconRemove;
        private GUIContent _iconSave;

        private string _newThemeId = string.Empty;

        [MenuItem("Window/Asset Management/Color Themes...", false, 200)]
        private static void OpenSettings()
        {
            ThemesWindow window = GetWindow<ThemesWindow>(utility: false, title: "Color Themes", focus: true);
            window.minSize = new Vector2(550.0f, 200.0f);
            window.Show();
        }

        private void OnEnable()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (_treeViewState == null)
                _treeViewState = new TreeViewState();

            MultiColumnHeaderState headerState = ThemesTreeView.CreateFixedMultiColumnHeaderState();
            if (MultiColumnHeaderState.CanOverwriteSerializedFields(_multiColumnHeaderState, headerState))
                MultiColumnHeaderState.OverwriteSerializedFields(_multiColumnHeaderState, headerState);
            _multiColumnHeaderState = headerState;

            MultiColumnHeader multiColumnHeader = new MultiColumnHeader(headerState);

            _themesTreeView = new ThemesTreeView(_treeViewState, multiColumnHeader);

            _iconToolbarPlus = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add color");
            _iconToolbarMinus = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove selected colors");

            _iconAddNew = EditorGUIUtility.TrIconContent("d_CreateAddNew", "Add new theme");
            _iconRename = EditorGUIUtility.TrIconContent("editicon.sml", "Rename selected theme");
            _iconRemove = EditorGUIUtility.TrIconContent("TreeEditor.Trash", "Remove selected theme");
            _iconSave = EditorGUIUtility.TrIconContent("SaveActive", "Save themes");
        }

        private void OnGUI()
        {
            if (EditorBuildSettings.TryGetConfigObject(ThemesSettings.ConfigName, out ThemesSettings settings))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    string[] themeIds = settings.GetThemeIds().ToArray();
                    if (themeIds.Length > 0)
                    {
                        GUIContent[] themeIdItems = themeIds.Select(themeId => new GUIContent(themeId)).ToArray();

                        int selectedThemeIndex = System.Array.IndexOf(themeIds, settings.CurrentThemeId);
                        if (selectedThemeIndex == -1) selectedThemeIndex = 0;

                        EditorGUI.BeginChangeCheck();
                        int newSelectedThemeIndex = EditorGUILayout.Popup(new GUIContent("Current theme"), selectedThemeIndex, themeIdItems, GUILayout.Width(300f));
                        if (EditorGUI.EndChangeCheck())
                        {
                            _newThemeId = themeIds[newSelectedThemeIndex];
                            ThemesSettingsEditor.SetCurrentThemeId(settings, _newThemeId);
                        }
                    }

                    GUILayout.FlexibleSpace();

                    _newThemeId = EditorGUILayout.TextField(_newThemeId, GUILayout.ExpandWidth(false));

                    using (new EditorGUI.DisabledScope(disabled: string.IsNullOrEmpty(_newThemeId) || settings.GetThemeIds().Contains(_newThemeId)))
                    {
                        if (GUILayout.Button(_iconRename, GUILayout.ExpandWidth(false)))
                            ThemesSettingsEditor.RenameTheme(settings, settings.CurrentThemeId, _newThemeId);

                        if (GUILayout.Button(_iconAddNew, GUILayout.ExpandWidth(false)))
                            ThemesSettingsEditor.AddTheme(settings, _newThemeId);
                    }

                    using (new EditorGUI.DisabledScope(disabled: string.IsNullOrEmpty(settings.CurrentThemeId)))
                    {
                        if (GUILayout.Button(_iconRemove, GUILayout.ExpandWidth(false)))
                            ThemesSettingsEditor.DeleteTheme(settings, settings.CurrentThemeId);
                    }

                    GUILayout.Space(20f);

                    using (new EditorGUI.DisabledScope(disabled: !EditorUtility.IsDirty(settings)))
                    {
                        if (GUILayout.Button(_iconSave, GUILayout.ExpandWidth(false)))
                            ThemesSettingsEditor.SaveAll(settings);
                    }
                }

                using (EditorGUILayout.HorizontalScope themesScope = new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: true)))
                {
                    MultiColumnHeaderState headerState = ThemesTreeView.UpdateMultiColumnHeaderState(_multiColumnHeaderState, settings);
                    if (MultiColumnHeaderState.CanOverwriteSerializedFields(_multiColumnHeaderState, headerState))
                        MultiColumnHeaderState.OverwriteSerializedFields(_multiColumnHeaderState, headerState);
                    else
                        _multiColumnHeaderState = headerState;

                    _themesTreeView.ThemesSettings = settings;
                    _themesTreeView.multiColumnHeader = new MultiColumnHeader(_multiColumnHeaderState);
                    _themesTreeView.Reload();
                    _themesTreeView.OnGUI(themesScope.rect);
                }

                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: false)))
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(_iconToolbarPlus, GUILayout.ExpandWidth(false)))
                        ThemesSettingsEditor.AddColor(settings, "NewColor");

                    using (new EditorGUI.DisabledScope(_treeViewState.selectedIDs.Count == 0))
                    {
                        if (GUILayout.Button(_iconToolbarMinus, GUILayout.ExpandWidth(false)))
                            foreach (int selectedId in _treeViewState.selectedIDs)
                            {
                                foreach (long colorId in settings.GetColorIds().ToArray())
                                    if ((int)colorId == selectedId)
                                        ThemesSettingsEditor.DeleteColor(settings, colorId);
                            }
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Create Themes"))
                {
                    string settingsPath = EditorUtility.SaveFilePanelInProject("Create Themes Settings", "ThemesSettings", "asset", "Create themes settings");
                    if (!string.IsNullOrEmpty(settingsPath))
                        ThemesSettingsEditor.CreateSettingsAndKeyTable(settingsPath);
                }
            }
        }
    }
}
