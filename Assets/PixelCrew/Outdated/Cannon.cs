using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components;

namespace PixelCrew
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private SpawnComponent _cannonBall;
        [SerializeField] private float _fireInterval;
        [SerializeField] private Animator _animator;

        [HideInInspector] public float _fireCountDown = 0f;

        private static readonly int Fire = Animator.StringToHash("fire");

        private void FixedUpdate()
        {
                if (_fireCountDown > 0f)
                {
                    _fireCountDown -= Time.deltaTime;
                }
                else if (_fireCountDown <= 0f)
                {
                    _animator.SetTrigger(Fire);
                    _fireCountDown = _fireInterval;
                }
        }

        public void SpawnCannonBall()
        {
            _cannonBall.Spawn();
        }
    }
}
