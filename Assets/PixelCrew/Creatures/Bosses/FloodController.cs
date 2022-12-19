using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses
{
    public class FloodController : MonoBehaviour
    {
        [SerializeField] private Animator _floodAnimator;
        [SerializeField] private float _floodTime;

        private Coroutine _coroutine;

        private static readonly int IsFlooding = Animator.StringToHash("isFlooding");

        public void StartFlooding()
        {
            if (_coroutine != null) return;

            _coroutine = StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _floodAnimator.SetBool(IsFlooding, true);
            yield return new WaitForSeconds(_floodTime);
            _floodAnimator.SetBool(IsFlooding, false);
            _coroutine = null;
        }
    }
}