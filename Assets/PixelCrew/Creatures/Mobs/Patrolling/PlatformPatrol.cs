using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _platformCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private OnChangeDirection _onChangeDirection;

        private Vector2 _direction = Vector2.one;

        public override IEnumerator DoPatrol()
        {
            _direction.y = 0f;

            while (enabled)
            {
                if (_platformCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
                {
                    _onChangeDirection?.Invoke(_direction);
                }
                else
                {
                    _direction *= (-1f);
                    _onChangeDirection?.Invoke(_direction);
                }

                yield return null;
            }

        }
    }

    [Serializable]
    public class OnChangeDirection : UnityEvent<Vector2>
    {
    }
}
