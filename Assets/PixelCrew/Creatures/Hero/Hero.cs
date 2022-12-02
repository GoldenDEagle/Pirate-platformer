using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Creatures
{

    public class Hero : Creature
    {
        [SerializeField] CheckCircleOverlap _interactionCheck;
        
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
        [SerializeField] private SpawnComponent _throwSpawner;


        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private bool _allowDoubleJump;
        private bool _megaThrow;

        private HealthComponent _health;
        private GameSession _session;

        private const string SwordId = "Sword";
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();

            _session.Data.Inventory.OnChanged += OnInventoryChanged;

            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override float CalculateYVelocity()  // Рассчет вертикальной скорости
        {
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
                DoJumpVfx();
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddToInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        //public void UseItem(string id)
        //{
        //    var itemCount = _session.Data.Inventory.Count(id);
        //    if (itemCount > 0)
        //    {
        //        _session.Data.Inventory.Remove(id, 1);
        //        if (id == "HealthPotion") _health.ModifyHealth(3);  // hp potion restoration value
        //    }
        //}

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        public override void TakeDamage()  // Получение урона
        {
            base.TakeDamage();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var coinsToDispose = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", coinsToDispose);

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


        public override void Attack()    // Анимация атаки
        {
            if (SwordCount <= 0) return;

            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }

        public void OnDoThrow()
        {
            if (_megaThrow)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                
                var numThrows = Mathf.Min(_megaThrowCount, possibleCount);
                StartCoroutine(MegaThrowRoutine(numThrows));
            }
            else
            {
                Throw();
            }

            _megaThrow = false;
        }
        
        public void Throw()
        {
            Sounds.Play("Range");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);

            _session.Data.Inventory.Remove(throwableId, 1);
            _throwSpawner.Spawn();

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
            if (!_throwCooldown.IsReady || !CanThrow) return;

            if (_megaThrowCooldown.IsReady) _megaThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
    }
}
