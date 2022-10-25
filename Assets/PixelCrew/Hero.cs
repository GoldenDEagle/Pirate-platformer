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
        private Animator _animator;
        private SpriteRenderer _sprite;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");                // Переменные для навигации по анимациям
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");

        public int _sumOfCoins;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();       // Подключение компонентов из Юнити
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);  // Движение вправо-влево

            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();

            if (isJumping)  // Описание прыжка
            {
                if (isGrounded && _rigidbody.velocity.y <= 0)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpspeed, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }

            _animator.SetBool(IsGroundKey, isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);                // Задание параметров для перехода анимаций
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection(); 

        }

        private void UpdateSpriteDirection()   // Функция смены направления спрайта
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
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
