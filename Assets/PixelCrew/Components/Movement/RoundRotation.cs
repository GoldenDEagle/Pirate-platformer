using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    public class RoundRotation : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Rigidbody _rigidbody;
        private Vector3 _angleVelocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _angleVelocity = new Vector3(0, 0, _speed);
        }

        void FixedUpdate()
        {
            Quaternion deltaRotation = Quaternion.Euler(_angleVelocity * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
    }
}