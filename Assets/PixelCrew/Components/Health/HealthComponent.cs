﻿using PixelCrew.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange;

        private Lock _immune = new Lock();

        public int Health => _health;
        public Lock Immune => _immune;

        public void ModifyHealth(int hpChange)
        {
            if (hpChange < 0 && Immune.IsLocked) return;
            if (_health <= 0) return;

            _health += hpChange;
            _onChange?.Invoke(_health);

            if (hpChange < 0)
            {
                _onDamage?.Invoke();
            }
            if (hpChange > 0)
            {
                _onHeal?.Invoke();
            }
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
        }
    }
}