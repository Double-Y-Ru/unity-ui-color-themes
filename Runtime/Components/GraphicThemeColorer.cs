using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Components
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class GraphicThemeColorer : ThemeSingleColorer
    {
        [SerializeField]
        private MaskableGraphic _graphic;

        public override Component GetTargetComponent() => _graphic;

        // called only in Editor
        private void OnValidate()
        {
            _graphic ??= GetComponent<MaskableGraphic>();
        }

        protected override Color GetColor() => _graphic.color;
        protected override void SetColor(Color color) => _graphic.color = color;
    }
}