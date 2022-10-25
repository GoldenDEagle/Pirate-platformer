using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{

    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpspeed;

        [SerializeField] private LayerCheck _groundCheck;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;

        public int _sumOfCoins;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            if (isJumping)
            {
                if (IsGrounded() && _rigidbody.velocity.y <= 0)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpspeed, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }

        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        public void CoinPickUp(int coinValue)
        {
            _sumOfCoins = _sumOfCoins + coinValue;
        }

    }
}
