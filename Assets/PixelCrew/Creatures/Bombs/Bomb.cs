using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Bombs
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _lifetime;
        [SerializeField] private UnityEvent _onDetonate;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            TryStop();
            _coroutine = StartCoroutine(WaitAndDetonate());
        }

        private IEnumerator WaitAndDetonate()
        {
            yield return new WaitForSeconds(_lifetime);
            Detonate();
            _coroutine = null;
        }

        public void Detonate()
        {
            _onDetonate?.Invoke();
        }

        private void TryStop()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = null;
        }

        private void OnDisable()
        {
            TryStop();
        }
    }
}