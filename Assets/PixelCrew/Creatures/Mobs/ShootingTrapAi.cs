using UnityEngine;
using PixelCrew.Components;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAi : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private bool _soloMode;

        [Header("Melee")] 
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private static readonly int MeleeKey = Animator.StringToHash("melee");
        private static readonly int RangeKey = Animator.StringToHash("range");

        private Animator _animator;
        private AudioSource _audio;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer && _soloMode)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                    {
                        MeleeAttack();
                    }
                    return;
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(MeleeKey);
        }

        public void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(RangeKey);
            _audio.Play();
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}