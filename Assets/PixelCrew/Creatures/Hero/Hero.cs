using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Model;

namespace PixelCrew.Creatures
{

    public class Hero : Creature
    {
        [SerializeField] CheckCircleOverlap _interactionCheck;
        
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _interactionRadius;

        [SerializeField] private Cooldown _throwCooldown;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [Space]
        [Header("MegaThrow")]
        [SerializeField] private Cooldown _megaThrowCooldown;
        [SerializeField] private int _megaThrowCount;
        [SerializeField] private float _megaThrowInterval;

        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private bool _allowDoubleJump;
        private bool _megaThrow;

        public GameSession _session;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override float CalculateYVelocity()  // Рассчет вертикальной скорости
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)   // Рассчет скорости прыжка(обычный и дабл)
        {
            if (!IsGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }


        public void CoinPickUp(int coinValue)  // Подсчет монет
        {
            _session.Data.Coins += coinValue;
            Debug.Log($"Total coins: {_session.Data.Coins}");
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        public override void TakeDamage()  // Получение урона
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var coinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= coinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = coinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)    // Относительная скорость при контакте с землей
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()    // Анимация атаки
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
            _session.Data.Swords += 1;
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }

        public void OnDoThrow()
        {
            if (_megaThrow)
            {
                var swordsToThrow = Mathf.Min(_megaThrowCount, _session.Data.Swords - 1);
                StartCoroutine(MegaThrowRoutine(swordsToThrow));
            }
            else
            {
                Throw();
            }

            _megaThrow = false;
        }
        
        public void Throw()
        {
            _session.Data.Swords -= 1;
            _particles.Spawn("Throw");

        }

        private IEnumerator MegaThrowRoutine(int swordsToThrow)
        {
            {
                for (int i = 1; i <= swordsToThrow; i++)
                {
                    Throw();
                    yield return new WaitForSeconds(_megaThrowInterval);
                }
            }
        }

        public void StartThrowing()
        {
            _megaThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || _session.Data.Swords <= 1) return;

            if (_megaThrowCooldown.IsReady) _megaThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }
    }
}
