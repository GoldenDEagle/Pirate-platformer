using UnityEngine;

namespace PixelCrew.Components
{
    public class HpModifierComponent : MonoBehaviour
    {
        [SerializeField] private int _hpChange;

        public void ModifyHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ModifyHealth(_hpChange);
            }
        }
    }
}
