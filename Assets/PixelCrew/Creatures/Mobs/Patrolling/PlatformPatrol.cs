using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _platformCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        private Creature _creature;
        private Vector2 _direction = Vector2.one;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            _direction.y = 0f;

            while (enabled)
            {
                if (_platformCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
                {
                    _creature.SetDirection(_direction);
                }
                else
                {
                    _direction *= (-1f);
                    _creature.SetDirection(_direction);
                }

                yield return null;
            }

        }
    }
}
