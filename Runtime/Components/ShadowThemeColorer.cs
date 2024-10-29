using UnityEngine;
using UnityEngine.UI;

namespace DoubleY.ColorThemes.Components
{
    [RequireComponent(typeof(Shadow))]
    public class ShadowThemeColorer : ThemeSingleColorer
    {
        [SerializeField]
        private Shadow _shadow;

        public override Component GetTargetComponent() => _shadow;

        // called only in Editor
        private void OnValidate()
        {
            _shadow ??= GetComponent<Shadow>();
        }

        protected override Color GetColor() => _shadow.effectColor;
        protected override void SetColor(Color color) => _shadow.effectColor = color;
    }
}
