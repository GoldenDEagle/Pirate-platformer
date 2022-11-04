using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    public class ParticleOnTrigger : MonoBehaviour
    {
        private ParticleSystem ps;
        private Hero _hero;

        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        void OnEnable()
        {
            ps = GetComponent<ParticleSystem>();
            _hero = FindObjectOfType<Hero>();
        }


        void OnParticleTrigger()
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];

            // get the particles which matched the trigger conditions this frame
            int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

            // iterate through the particles which entered the trigger
            for (int i = 0; i < numEnter; i++)
            {
                _hero.CoinPickUp(1);
            }

        }
    }
}
