using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Components
{
    [RequireComponent(typeof(Selectable))]
    public class SelectableThemeColorer : ThemeBlockColorer
    {
        [SerializeField]
        private Selectable _selectable;

        public override Component GetTargetComponent() => _selectable;

        protected override ColorBlock GetColorBlock() => _selectable.colors;
        protected override void SetColorBlock(ColorBlock colorBlock) => _selectable.colors = colorBlock;
    }
}