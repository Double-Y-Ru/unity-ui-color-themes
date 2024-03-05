using DoubleY.ColorThemes.Components;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Editor.Components
{
    [CustomEditor(typeof(Text), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class TextEditorWithUseThemeButton : TextEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ThemeColoredComponentUtility.CheckAndAddUseThemeButton<ThemeColoredGraphic>(targets.Select(obj => obj as Graphic));
        }
    }
}
