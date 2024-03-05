using DoubleY.ColorThemes.Components;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Editor.Components
{
    [CustomEditor(typeof(Image), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class ImageEditorWithUseThemeButton : ImageEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ThemeColoredComponentUtility.CheckAndAddUseThemeButton<ThemeColoredGraphic>(targets.Select(obj => obj as Graphic));
        }
    }
}
