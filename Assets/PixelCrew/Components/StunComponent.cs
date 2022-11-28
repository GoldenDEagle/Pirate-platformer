using System.Collections;
using UnityEngine;
using PixelCrew.Utils;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class StunComponent : MonoBehaviour
    {
        [SerializeField] private float _stunDuration;
        [SerializeField] private CheckCircleOverlap _stunRadius;

        public void StunInArea()
        {
            _stunRadius.Check();
        }

        public void StunTarget(GameObject target)
        {
            var playerInput = target.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                StartCoroutine(StunRoutine(target));
            }
        }

        private IEnumerator StunRoutine(GameObject target)
        {
            var playerInput = target.GetComponent<PlayerInput>();

            playerInput.enabled = false;
            yield return new WaitForSeconds(_stunDuration);
            playerInput.enabled = true;
        }
    }
}