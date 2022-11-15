using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
