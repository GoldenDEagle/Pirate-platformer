using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.Bosses
{
    public class ChangeLightsComponent : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;
        [ColorUsage(true, true)]
        [SerializeField] private Color _color;

        private Color _defaultColor;

        private void Start()
        {
            _defaultColor = _lights[0].color;
        }

        public void SetColor()
        {
            SetColor(_color);
        }

        public void SetColor(Color color)
        {
            foreach (var light2D in _lights)
            {
                light2D.color = color;
            }
        }

        public void ResetColor()
        {
            foreach (var light2D in _lights)
            {
                light2D.color = _defaultColor;
            }
        }
    }
}