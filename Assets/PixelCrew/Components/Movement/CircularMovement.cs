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
        private Vector2[] currentPos;

        private void Awake()
        {
            _rigidbody = GetComponentsInChildren<Rigidbody2D>();
            _delta = (2 * Mathf.PI) / _rigidbody.Length;

        }

        private void Start()
        {
            currentPos = new Vector2[_rigidbody.Length];
            for (int i = 0; i < _rigidbody.Length; i++)
            {
                currentPos[i] = _rigidbody[i].position;
                currentPos[i].y = transform.position.y + Mathf.Sin(_delta * i) * _radius;
                currentPos[i].x = transform.position.x + Mathf.Cos(_delta * i) * _radius;
                _rigidbody[i].MovePosition(currentPos[i]);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _rigidbody.Length; i++)
            {
                if (_rigidbody[i] == null) continue;
                currentPos[i] = _rigidbody[i].position;
                currentPos[i].y = transform.position.y + Mathf.Sin(Time.time * _speed + _delta * i) * _radius;
                currentPos[i].x = transform.position.x + Mathf.Cos(Time.time * _speed + _delta * i) * _radius;
                _rigidbody[i].MovePosition(currentPos[i]);
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