using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] SpawnSequence[] _stages;

        public int Stage { get; set; }

        [ContextMenu("Launch")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var sequence = _stages[Stage];
            foreach (var setting in sequence.Sequence)
            {
                var sectorStep = 2 * Mathf.PI / setting.BurstCount;
                for (int i = 0, burstCount = 1; i < setting.BurstCount; i++, burstCount++)
                {
                    var angle = sectorStep * i;
                    var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                    var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(direction);

                    if (burstCount < setting.ItemsPerBurst) continue;

                    burstCount = 0;
                    yield return new WaitForSeconds(setting.Delay);
                }
            }
        }
    }

    [Serializable]
    public struct SpawnSequence
    {
        [SerializeField] private CircularProjectileSettings[] _sequence;

        public CircularProjectileSettings[] Sequence => _sequence;
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _burstCount;
        [SerializeField] private int _itemsPerBurst;
        [SerializeField] private float _delay;

        public DirectionalProjectile Prefab => _prefab;
        public int BurstCount => _burstCount;
        public int ItemsPerBurst => _itemsPerBurst;
        public float Delay => _delay;
    }
}