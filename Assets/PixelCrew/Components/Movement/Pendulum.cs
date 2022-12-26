using UnityEngine;

namespace Assets.PixelCrew.Components.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pendulum : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _leftAngle;
        [SerializeField] private float _rightAngle;
        [SerializeField] private bool _movingClockwise = false;

        private Rigidbody2D _rigidbody;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Move();
        }

        private void SetDirection()
        {
            if (transform.rotation.z > _rightAngle)
                _movingClockwise = true;
            if (transform.rotation.z < _leftAngle)
                _movingClockwise = false;
        }

        private void Move()
        {
            SetDirection();

            if (!_movingClockwise)
                _rigidbody.angularVelocity = _speed;
            else
                _rigidbody.angularVelocity = -_speed;
        }
    }
}