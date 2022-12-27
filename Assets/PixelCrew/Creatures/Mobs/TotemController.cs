using System.Collections;
using System.Linq;
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
        private Coroutine _coroutine;
        private int _destroyed;

        private void Awake()
        {
            _traps = GetComponentsInChildren<ShootingTrapAi>();
        }

        private void Update()
        {
            DestructionCheck();

            if (_coroutine == null)
            {
                if (_vision.IsTouchingLayer && _attackCooldown.IsReady)
                {
                    _coroutine = StartCoroutine(AttackSequence());
                }
            }
        }

        private void DestructionCheck()
        {
            foreach (var trap in _traps)
            {
                if (trap == null)
                    _destroyed++;
            }

            if (_destroyed == _traps.Length)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
            else _destroyed = 0;
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

            _coroutine = null;
        }
    }
}