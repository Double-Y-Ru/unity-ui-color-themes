using UnityEngine;

namespace DoubleY.ColorThemes.Components
{
    public abstract class ThemeColoredComponent : MonoBehaviour
    {
        // Some Components may not be initialized in Awake yet, so we apply colors in Start
        protected virtual void Start()
        {
            ApplyColors();
        }

        public abstract Component GetTargetComponent();
        public abstract void ApplyColors();
        public abstract bool TryFindSimilarThemeColorAndApply();
    }
}