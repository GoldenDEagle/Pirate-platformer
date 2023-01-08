using PixelCrew.Components;
using PixelCrew.Effects.CameraEffects;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private ColliderCheck _piercablePlatformCheck;

        [SerializeField] CheckCircleOverlap _interactionCheck;
        
        [SerializeField] private float _interactionRadius;

        [SerializeField] private Cooldown _meleeCooldown;

        [Header("Abilities")]
        [SerializeField] private ShieldComponent _shield;
        [SerializeField] private FlashlightComponent _flashlight;
        [SerializeField] private Cooldown _throwCooldown;

        [Space]
        [Header("MegaThrow")]
        [SerializeField] private Cooldown _megaThrowCooldown;
        [SerializeField] private int _megaThrowCount;
        [SerializeField] private float _megaThrowInterval;

        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private int _coinsDropped;
        [SerializeField] private SpawnComponent _throwSpawner;

        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _disarmed;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private bool _allowDoubleJump;
        private bool _megaThrow;

        private Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;
        private int _meleeDamage;

        private HealthComponent _health;
        private CameraShakeEffect _cameraShake;
        private TempColliderDisable _colliderDisabler;

        [HideInInspector] public bool IsCrawling;

        private const string SwordId = "Sword";
        private const string CoinId = "Coin";
        private const string RangeId = "Range";
        private int CoinCount => GameSession.Instance.Data.Inventory.Count(CoinId);
        private int SwordCount => GameSession.Instance.Data.Inventory.Count(SwordId);

        private string SelectedItemId => GameSession.Instance.QuickInventory.SelectedItem.Id;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _colliderDisabler = GetComponent<TempColliderDisable>();
            _health = GetComponent<HealthComponent>();
            _cameraShake = FindObjectOfType<CameraShakeEffect>() ?? null;

            GameSession.Instance.Data.Inventory.OnChanged += OnInventoryChanged;
            GameSession.Instance.StatsModel.OnUpgraded += OnHeroUpgraded;

            _health.SetHealth(GameSession.Instance.Data.Hp.Value);
            _meleeDamage = (int) GameSession.Instance.StatsModel.GetValue(StatId.MeleeDamage);
            _meleeAttack.SetDelta(-_meleeDamage);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
            if (_piercablePlatformCheck.IsTouchingLayer && IsCrawling)
                _colliderDisabler.DisableCollider();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int)GameSession.Instance.StatsModel.GetValue(statId);
                    GameSession.Instance.Data.Hp.Value = health;
                    UpdateHealth();
                    _health.SetHealth(health);
                    break;
                case StatId.MeleeDamage:
                    _meleeDamage = (int) GameSession.Instance.StatsModel.GetValue(statId);
                    _meleeAttack.SetDelta(-_meleeDamage);
                    break;
                case StatId.Fuel:
                    GameSession.Instance.Data.Fuel.Value = GameSession.Instance.StatsModel.GetValue(statId);
                    break;
                case StatId.CooldownReduction:
                    var usedPerk = GameSession.Instance.PerksModel.Used;
                    if (usedPerk == null) break;
                    var defaultCooldown = GameSession.Instance.PerksModel.GetPerkCooldown(usedPerk);
                    var cooldownReduction = GameSession.Instance.StatsModel.GetValue(StatId.CooldownReduction);
                    GameSession.Instance.PerksModel.Cooldown.Value = (1 - cooldownReduction / 100) * defaultCooldown;
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
            if (!IsGrounded && _allowDoubleJump && GameSession.Instance.PerksModel.IsDoubleJumpSupported)
            {
                GameSession.Instance.PerksModel.Cooldown.Reset();
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        protected override float CalculateSpeed()
        {
            var defaultSpeed = GameSession.Instance.StatsModel.GetValue(StatId.Speed);

            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            return defaultSpeed + _additionalSpeed;
        }

        public void AddToInventory(string id, int value)
        {
            GameSession.Instance.Data.Inventory.Add(id, value);
        }

        public void OnHealthChanged(int currentHealth)
        {
            GameSession.Instance.Data.Hp.Value = currentHealth;
        }

        private void UpdateHealth()
        {
            if (GameSession.Instance.Data.Hp.Value > GameSession.Instance.StatsModel.GetValue(StatId.Hp)) GameSession.Instance.Data.Hp.Value = (int) GameSession.Instance.StatsModel.GetValue(StatId.Hp);
            _health.SetHealth(GameSession.Instance.Data.Hp.Value);
        }

        public override void TakeDamage()  // Получение урона
        {
            base.TakeDamage();
            _cameraShake?.Shake();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
            UpdateHealth();
        }

        private void SpawnCoins()
        {
            var coinsToDispose = Mathf.Min(CoinCount, _coinsDropped);
            GameSession.Instance.Data.Inventory.Remove(CoinId, coinsToDispose);

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
            if ((SwordCount <= 0) || (!_meleeCooldown.IsReady)) return;

            var damageValue = ModifyDamageByCrit(_meleeDamage);
            _meleeAttack.SetDelta(-damageValue);

            _meleeCooldown.Reset();
            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }

        public void OnDoThrow()
        {
            if (_megaThrow && GameSession.Instance.PerksModel.IsMegaThrowSupported)
            {
                var throwableCount = GameSession.Instance.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                
                var numThrows = Mathf.Min(_megaThrowCount, possibleCount);
                GameSession.Instance.PerksModel.Cooldown.Reset();
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
            Sounds.Play(RangeId);

            var throwableId = GameSession.Instance.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);

            GameSession.Instance.Data.Inventory.Remove(throwableId, 1);
            var instance = _throwSpawner.SpawnInstance();
            ApplyRangeDamageStat(instance);
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
            projectile.TryGetComponent<HpModifierComponent>(out HpModifierComponent hpModifier);
            var damageValue = (int)GameSession.Instance.StatsModel.GetValue(StatId.RangeDamage);
            damageValue = ModifyDamageByCrit(damageValue);
            hpModifier.SetDelta(- damageValue);
        }

        private int ModifyDamageByCrit(int damage)
        {
            var critChance = GameSession.Instance.StatsModel.GetValue(StatId.CritChance);
            if (Random.value * 100 <= critChance)
            {
                return damage * 2;
            }
            return damage;
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
            return GameSession.Instance.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    GameSession.Instance.Data.Hp.Value += (int)potion.Value;
                    UpdateHealth();
                    break;
                case Effect.SpeedUp:
                    _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }

            GameSession.Instance.Data.Inventory.Remove(potion.Id, 1);
        }

        private void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThrow) return;

            if (_megaThrowCooldown.IsReady) _megaThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        public void NextItem()
        {
            GameSession.Instance.QuickInventory.SetNextItem();
        }

        public void UsePerk()
        {
            if (GameSession.Instance.PerksModel.IsShieldSupported)
            {
                _shield.Use();
                GameSession.Instance.PerksModel.Cooldown.Reset();
            }
        }

        public void ToggleFlashlight()
        {
            var isActive = _flashlight.gameObject.activeSelf;
            _flashlight.gameObject.SetActive(!isActive);
        }

        private void OnDestroy()
        {
            GameSession.Instance.Data.Inventory.OnChanged -= OnInventoryChanged;
            GameSession.Instance.StatsModel.OnUpgraded -= OnHeroUpgraded;
        }
    }
}
