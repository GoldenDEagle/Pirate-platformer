using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Model;

namespace PixelCrew.Components.LevelManagment
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckpointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private SpawnComponent _heroSpawner;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnChecked;

        public string Id => _id;

        private void Start()
        {
            if (GameSession.Instance.IsChecked(_id))
            {
                _setChecked?.Invoke();
            }
            else
            {
                _setUnChecked?.Invoke();
            }
        }

        public void Check()
        {
            GameSession.Instance.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawner.Spawn();
        }
    }
}