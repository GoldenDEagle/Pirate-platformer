using UnityEngine;

namespace PixelCrew.Outdated
{
    public class CannonBallBehavior : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifetime;
        [SerializeField] [Range(-1, 1)] private float _direction;

        private void Awake()
        {
            Destroy(gameObject, _lifetime);
        }

        private void Update()
        {
            var delta = _direction * _speed * Time.deltaTime;
            transform.position = transform.position + new Vector3(delta, 0, 0);
        }

    }

}
