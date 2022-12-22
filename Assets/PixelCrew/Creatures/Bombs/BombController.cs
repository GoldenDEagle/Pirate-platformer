using PixelCrew.Components;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Bombs
{
    public class BombController : MonoBehaviour
    {
        [SerializeField] private BombSequence[] _sequences;

        private Coroutine _coroutine;

        [ContextMenu("Start bombing!")]
        public void StartBombing()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(BombingSequence());
        }

        private IEnumerator BombingSequence()
        {
            foreach (var bombSequence in _sequences)
            {
                foreach (var spawnComponent in bombSequence.Spawners)
                {
                    spawnComponent.Spawn();
                }

                yield return new WaitForSeconds(bombSequence.Delay);
            }
        }

        [Serializable]
        public class BombSequence
        {
            [SerializeField] private SpawnComponent[] _spawners;
            [SerializeField] private float _delay;

            public SpawnComponent[] Spawners => _spawners;
            public float Delay => _delay;
        }
    }
}