using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DoubleY.ColorThemes.Editor
{
    public class ThemesTreeView : TreeView
    {
        public ThemesSettings ThemesSettings { get; set; }

        public ThemesTreeView(TreeViewState state, MultiColumnHeader multiColumnHeaderState) : base(state, multiColumnHeaderState)
        {
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root", children = new List<TreeViewItem>(0) };

            if (ThemesSettings != null)
                foreach (long colorId in ThemesSettings.GetColorIds())
                    root.AddChild(new ThemesTreeViewItem(ThemesSettings, colorId));

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            ThemesTreeViewItem item = (ThemesTreeViewItem)args.item;
            for (int visibleColumnIndex = 0; visibleColumnIndex < args.GetNumVisibleColumns(); ++visibleColumnIndex)
                CellGUI(args.GetCellRect(visibleColumnIndex), item, args.GetColumn(visibleColumnIndex));
        }

        private void CellGUI(Rect cellRect, ThemesTreeViewItem item, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                    EditorGUI.LabelField(cellRect, item.ThemeColorId.ToString(), new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight });
                    break;
                case 1:
                    EditorGUI.BeginChangeCheck();
                    string newColorName = EditorGUI.TextField(cellRect, item.ColorName);
                    if (EditorGUI.EndChangeCheck())
                        ThemesSettingsEditor.SetColorName(item.ThemesSettings, item.ThemeColorId, newColorName);
                    break;
                default:
                    const int firstColorColumnIndex = 2;
                    string[] themeIds = ThemesSettings.GetThemeIds().ToArray();
                    int themeIndex = columnIndex - firstColorColumnIndex;
                    string themeId = themeIds[themeIndex];

                    EditorGUI.BeginChangeCheck();
                    Color newColor = EditorGUI.ColorField(cellRect, item.GetColor(themeId));
                    if (EditorGUI.EndChangeCheck())
                        ThemesSettingsEditor.SetColor(item.ThemesSettings, themeId, item.ThemeColorId, newColor);
                    break;
            }
        }

        public static MultiColumnHeaderState CreateFixedMultiColumnHeaderState()
        {
            List<MultiColumnHeaderState.Column> columns = new List<MultiColumnHeaderState.Column>
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Id"),
                    headerTextAlignment = TextAlignment.Right,
                    width = 150,
                    minWidth = 60,
                    autoResize = true,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Theme color name"),
                    headerTextAlignment = TextAlignment.Right,
                    width = 150,
                    minWidth = 60,
                    autoResize = true,
                    allowToggleVisibility = false
                }
            };

            return new MultiColumnHeaderState(columns.ToArray());
        }

        public static MultiColumnHeaderState UpdateMultiColumnHeaderState(MultiColumnHeaderState currentState, ThemesSettings settings)
        {
            IEnumerable<MultiColumnHeaderState.Column> fixedColumns = currentState.columns.Take(2);
            MultiColumnHeaderState.Column[] themeColumns = currentState.columns.Skip(2).ToArray();

            List<MultiColumnHeaderState.Column> newThemesColumns = new List<MultiColumnHeaderState.Column>();

            foreach (string themeId in settings.GetThemeIds())
            {
                MultiColumnHeaderState.Column existingColumn = themeColumns.FirstOrDefault(c => c.headerContent.text == themeId);
                if (existingColumn == null)
                    newThemesColumns.Add(CreateThemeColumnState(themeId));
                else
                    newThemesColumns.Add(existingColumn);
            }
            return new MultiColumnHeaderState(fixedColumns.Concat(newThemesColumns).ToArray());
        }

        private static MultiColumnHeaderState.Column CreateThemeColumnState(string themeId)
        {
            return new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent(themeId),
                headerTextAlignment = TextAlignment.Center,
                width = 110,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = false
            };
        }
    }
}
