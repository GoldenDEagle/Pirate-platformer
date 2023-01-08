using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures
{
    public class FlashlightComponent : MonoBehaviour
    {
        [SerializeField] private float _consumePerSecond;
        [Range(0,1)]
        [SerializeField] private float _dimTreshold;
        [SerializeField] private Light2D _light;

        private float _defaultIntensity;

        private void Start()
        {
            _defaultIntensity = _light.intensity;
        }

        private void Update()
        {
            var consumed = Time.deltaTime * _consumePerSecond;
            var currentValue = GameSession.Instance.Data.Fuel.Value;
            var newValue = currentValue - consumed;
            newValue = Mathf.Max(newValue, 0);
            GameSession.Instance.Data.Fuel.Value = newValue;
            var dimTreshold = GameSession.Instance.StatsModel.GetValue(StatId.Fuel) * _dimTreshold;

            var progress = Mathf.Clamp(newValue / dimTreshold, 0.3f, 1);
            _light.intensity = _defaultIntensity * progress;
        }
    }
}