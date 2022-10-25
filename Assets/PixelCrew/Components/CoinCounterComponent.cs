using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class CoinCounterComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _coinToCount;
        [SerializeField] private int _coinValue;
        [SerializeField] private Hero _hero;

        public void CountCoin()
        {
            _hero.CoinPickUp(_coinValue);
            Debug.Log($"Total coins: {_hero._sumOfCoins}");
        }

    }
}
