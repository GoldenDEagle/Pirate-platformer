using PixelCrew.Creatures;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.LevelManagment
{
    public class ShipController : MonoBehaviour
    {
        [SerializeField] private bool _arrival;
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;
        private InputLock _inputLock;
        private Coroutine _coroutine;
        private bool isMoving;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _inputLock = GetComponent<InputLock>();
            if (_arrival)
            {
                StartFloating();
            }
        }

        public void StartFloating()
        {
            _inputLock.SetInput(false);
            _coroutine = StartCoroutine(Float());
        }

        private IEnumerator Float()
        {
            var hero = FindObjectOfType<Hero>();
            isMoving = true;
            while (isMoving)
            {
                var position = _rigidbody.position;
                position.x += _speed;
                _rigidbody.MovePosition(position);
                hero.transform.position = new Vector2(position.x, position.y + 0.7f);
                yield return null;
            }
        }

        public void StopFloating()
        {
            isMoving = false;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }
}