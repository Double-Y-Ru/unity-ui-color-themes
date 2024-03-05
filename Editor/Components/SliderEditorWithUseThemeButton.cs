using DoubleY.ColorThemes.Components;
using System.Linq;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Editor.Components
{
    [CustomEditor(typeof(Slider), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class SliderEditorWithUseThemeButton : SliderEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ThemeColoredComponentUtility.CheckAndAddUseThemeButton<ThemeColoredSelectable>(targets.Select(obj => obj as Slider));
        }
    }
}
