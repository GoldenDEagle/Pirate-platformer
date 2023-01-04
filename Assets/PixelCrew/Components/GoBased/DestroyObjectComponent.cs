using PixelCrew.Components.GoBased;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{

    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private RestoreStateComponent _state;

        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
            if (_state != null)
                GameSession.Instance.StoreDestructionState(_state.Id);
        }
    }
}
