using PixelCrew.Model;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;

        private void Start()
        {
            var isDestroyed = GameSession.Instance.RestoreDestructionState(Id);
            if (isDestroyed)
                Destroy(gameObject);
        }
    }
}