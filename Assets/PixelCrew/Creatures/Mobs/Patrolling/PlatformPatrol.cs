﻿using System;
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

        private Vector2 _direction;

        public override IEnumerator DoPatrol()
        {
            if (UnityEngine.Random.value < 0.5f)
                _direction = Vector2.right;
            else
                _direction = Vector2.left;

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
