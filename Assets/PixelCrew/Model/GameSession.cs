using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using PixelCrew.Components.LevelManagment;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckpoint;

        public PlayerData Data => _data;
        private PlayerData _save;

        private readonly List<string> _checkpoints = new List<string>();

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }

        private void Awake()
        {
            var existingSession = GetExistingSession();
            if (existingSession != null)
            {
                existingSession.StartSession(_defaultCheckpoint);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();

                DontDestroyOnLoad(this);
                StartSession(_defaultCheckpoint);
            }
        }

        private void StartSession(string defaultCheckpoint)
        {
            SetChecked(defaultCheckpoint);

            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckpointComponent>();
            var lastCheckpoint = _checkpoints.Last();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Id == lastCheckpoint)
                {
                    checkpoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }


        private GameSession GetExistingSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    return gameSession;
                }
            }

            return null;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();

            _trash.Dispose();
            InitModels();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
