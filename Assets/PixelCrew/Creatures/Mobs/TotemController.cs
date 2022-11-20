using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemController : MonoBehaviour
    {
        [SerializeField] float _shotInterval;
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private Cooldown _attackCooldown;

        private ShootingTrapAi[] _traps;

        private void Awake()
        {
            _traps = GetComponentsInChildren<ShootingTrapAi>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer && _attackCooldown.IsReady)
            {
                StartCoroutine(AttackSequence());
            }
        }

        private IEnumerator AttackSequence()
        {
            _attackCooldown.Reset();

            for (int i = 0; i < _traps.Length; i++)
            {
                if (_traps[i] != null)
                {
                    _traps[i].RangeAttack();
                    yield return new WaitForSeconds(_shotInterval);
                }
            }
        }
    }
}