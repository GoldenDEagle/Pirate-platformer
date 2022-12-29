using UnityEngine;

namespace PixelCrew.Creatures.Bosses
{
    public class TentacleAI : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Patrol _patrol;
        [SerializeField] private float _lifetime = 5;

        private void Start()
        {
            StartCoroutine(_patrol.DoPatrol());
            Destroy(gameObject, _lifetime);
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _direction * _speed;
        }

        private void Update()
        {
            if (_direction.x > 0)
                transform.localScale = new Vector3(-1,  1, 1);
            else if (_direction.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }
}