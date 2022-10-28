using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class CoinCounterComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _coinToCount;
        [SerializeField] private int _coinValue;
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void CountCoin()
        {
            _hero.CoinPickUp(_coinValue);
        }

    }
}
