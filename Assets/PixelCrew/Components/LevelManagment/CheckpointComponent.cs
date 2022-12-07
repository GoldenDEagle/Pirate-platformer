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

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            if (_session.IsChecked(_id))
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
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawner.Spawn();
        }
    }
}