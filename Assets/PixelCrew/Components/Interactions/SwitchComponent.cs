using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{

    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animationKey;
        [SerializeField] private bool _updateOnStart;

        private void Start()
        {
            if ((GameSession.Instance.WasSwitched(_id)) && !_updateOnStart)
            {
                _state = GameSession.Instance.RestoreSwitchState(_id);
                _animator.SetBool(_animationKey, _state);
            }
            else if (_updateOnStart)
            {
                _animator.SetBool(_animationKey, _state);
                GameSession.Instance.StoreSwitchState(_id, _state);
            }
        }

        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animationKey, _state);
            GameSession.Instance.StoreSwitchState(_id, _state);
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }

}
