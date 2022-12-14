using System.Collections;
using UnityEngine;
using UnityEditor.Animations;
using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;

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

        private Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;

        private HealthComponent _health;
        private GameSession _session;

        private const string SwordId = "Sword";
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;

            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int) _session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = health;
                    UpdateHealth();
                    _health.SetHealth(health);
                    break;
            }
        }

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

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
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
            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported)
            {
                _session.PerksModel.Cooldown.Reset();
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        protected override float CalculateSpeed()
        {
            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);

            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            return defaultSpeed + _additionalSpeed;
        }

        public void AddToInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        private void UpdateHealth()
        {
            if (_session.Data.Hp.Value > _session.StatsModel.GetValue(StatId.Hp)) _session.Data.Hp.Value = (int)_session.StatsModel.GetValue(StatId.Hp);
            _health.SetHealth(_session.Data.Hp.Value);
        }

        public override void TakeDamage()  // Получение урона
        {
            base.TakeDamage();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
            UpdateHealth();
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
            if (_megaThrow && _session.PerksModel.IsMegaThrowSupported)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                
                var numThrows = Mathf.Min(_megaThrowCount, possibleCount);
                _session.PerksModel.Cooldown.Reset();
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

        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable))
            {
                PerformThrowing();
            }
            else if (IsSelectedItem(ItemTag.Potion))
            {
                UsePotion();
            }
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    _session.Data.Hp.Value += (int)potion.Value;
                    UpdateHealth();
                    break;
                case Effect.SpeedUp:
                    _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }

            _session.Data.Inventory.Remove(potion.Id, 1);
        }

        private void PerformThrowing()
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
