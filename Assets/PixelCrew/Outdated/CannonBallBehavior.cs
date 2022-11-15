using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew
{

    public class CannonBallBehavior : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifetime;
        [SerializeField] [Range(-1, 1)] private float _direction;

        private void Awake()
        {
            Destroy(this.gameObject, _lifetime);
        }

        private void Update()
        {
            var delta = _direction * _speed * Time.deltaTime;
            transform.position = transform.position + new Vector3(delta, 0, 0);
        }

    }

}
