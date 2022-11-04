using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew.Components
{

    public class CoinDropComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _coinDropParticles;
        [SerializeField] [Range(1, 5)] private int _maxCoins = 5;

        public void SpawnCoins()
        {
            System.Random rnd = new System.Random();
            var coinsToDispose = rnd.Next(1, _maxCoins);

            var burst = _coinDropParticles.emission.GetBurst(0);
            burst.count = coinsToDispose;
            _coinDropParticles.emission.SetBurst(0, burst);

            _coinDropParticles.gameObject.SetActive(true);
            _coinDropParticles.Play();

            Transform child = this.gameObject.FindChildTransform("CoinDrop");
            child.parent = null;
        }
    }
}
