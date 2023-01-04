using UnityEngine;

namespace PixelCrew.Effects
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float _effectValue;
        [SerializeField] private Transform _followedTarget;

        private float _startX;

        private void Start()
        {
            _startX = transform.position.x;
        }

        private void FixedUpdate()
        {
            var currentPosition = transform.position;
            var deltaX = _followedTarget.position.x * _effectValue;
            transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
        }
    }
}
