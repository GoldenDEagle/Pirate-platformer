using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShieldComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Cooldown _duration;

        public void Use()
        {
            _healthComponent.Immune = true;
            _duration.Reset();
            gameObject.SetActive(true); ;
        }

        private void Update()
        {
            if (_duration.IsReady)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _healthComponent.Immune = false;
        }
    }
}