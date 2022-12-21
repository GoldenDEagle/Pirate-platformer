using UnityEngine;
using PixelCrew.Components;
using PixelCrew.Components.Audio;
using PixelCrew.Utils;
using UnityEngine.Profiling;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] protected float _slamDownVelocity;
        [SerializeField] protected bool _stunningSlam;

        [Header("Checkers")]
        [SerializeField] private ColliderCheck _groundCheck;
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        [SerializeField] protected HpModifierComponent _meleeAttack;

        protected Vector2 Direction;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected StunComponent Stunner;
        protected PlaySoundsComponent Sounds;
        protected bool IsGrounded;
        private bool _isJumping;
        private bool _isStunning;
        private float xVelocity;
        private float yVelocity;

        public bool StunningSlam => _stunningSlam;
        public float Speed => _speed;


        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");                // Переменные для навигации по анимациям
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        private static readonly int SlamdownKey = Animator.StringToHash("slamdown");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();       // Подключение компонентов из Юнити
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
            _isStunning = TryGetComponent<StunComponent>(out Stunner);
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        private void FixedUpdate()
        {
            xVelocity = CalculateXVelocity();   // Определение скорости x и y в каждый апдейт
            yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);


            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetBool(IsRunning, Direction.x != 0);                // Задание параметров для перехода анимаций
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);

            UpdateSpriteDirection(Direction);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)    // Относительная скорость при контакте с землей
                {
                    Animator.SetTrigger(SlamdownKey);
                    _particles.Spawn("SlamDown"); 

                    if (_stunningSlam && _isStunning)
                    {
                        Stunner.StunInArea();
                        Sounds.Play("Slam");
                    }
                }
            }
        }

        protected virtual float CalculateXVelocity()
        {
            return Direction.x * CalculateSpeed();
        }

        protected virtual float CalculateSpeed()
        {
            return _speed;
        }

        protected virtual float CalculateYVelocity()  // Рассчет вертикальной скорости
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;

                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)   // Рассчет скорости прыжка
        {
            if (IsGrounded)
            {
                yVelocity += _jumpSpeed;
                DoJumpVfx();
            }

            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
        }

        public void UpdateSpriteDirection(Vector2 direction)   // Функция смены направления спрайта
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage()  // Получение урона
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
        }

        public virtual void Attack()    // Анимация атаки
        {
            Animator.SetTrigger(AttackKey);
        }

        public void OnDoAttack()    // Нанесение урона
        {
            _attackRange.Check();
            _particles.Spawn("Attack1");
            Sounds.Play("Melee");
        }

        public void JumpOnTarget()
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _jumpSpeed);
        }
    }
}
