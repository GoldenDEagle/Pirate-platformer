using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _speed;

        private Rigidbody2D[] _rigidbody;
        private float _delta;
        private Vector2[] _positions;

        private void Awake()
        {
            UpdateContent();
            _delta = (2 * Mathf.PI) / _rigidbody.Length;
            _positions = new Vector2[_rigidbody.Length];
        }

        private void UpdateContent()
        {
            _rigidbody = GetComponentsInChildren<Rigidbody2D>();
        }

        private void Update()
        {
            CalculatePositions();
            var areAllDead = true;
            for (int i = 0; i < _rigidbody.Length; i++)
            {
                if (_rigidbody[i] == null) continue;
                else
                {
                    _rigidbody[i].MovePosition(_positions[i]);
                    areAllDead = false;
                }
            }

            if (areAllDead)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
        }

        private void CalculatePositions()
        {
            for (int i = 0; i < _rigidbody.Length; i++)
            {
                if (_rigidbody[i] == null) continue;
                _positions[i] = _rigidbody[i].position;
                _positions[i].y = transform.position.y + Mathf.Sin(Time.time * _speed + _delta * i) * _radius;
                _positions[i].x = transform.position.x + Mathf.Cos(Time.time * _speed + _delta * i) * _radius;
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
#endif
    }
}