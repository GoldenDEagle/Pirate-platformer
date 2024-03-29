﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using PixelCrew.Components.LevelManagment;
using PixelCrew.Model.Definitions;
using System.Diagnostics;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckpoint;

        public static GameSession Instance { get; private set; }

        public PlayerData Data => _data;
        private PlayerData _save;

        private readonly List<string> _checkpoints = new List<string>();
        private readonly List<string> _removedItems = new List<string>();
        private readonly Dictionary<string, bool> _switchesStates = new Dictionary<string, bool>();

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }

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
                Instance = this;
                StartSession(_defaultCheckpoint);
            }
        }

        private void StartSession(string defaultCheckpoint)
        {
            SetChecked(defaultCheckpoint);

            LoadUIs();
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

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int) StatsModel.GetValue(StatId.Hp);
            _data.Fuel.Value = StatsModel.GetValue(StatId.Fuel);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
            LoadControls();
        }

        [Conditional("MOBILE_BUILD")]
        private void LoadControls()
        {
            SceneManager.LoadScene("OnScreenControls", LoadSceneMode.Additive);
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

        public void StoreDestructionState(string itemId)
        {
            if (!_removedItems.Contains(itemId))
                _removedItems.Add(itemId);
        }

        public bool RestoreDestructionState(string itemId)
        {
            return _removedItems.Contains(itemId);
        }

        public void StoreSwitchState(string itemId, bool state)
        {
            if (!_switchesStates.ContainsKey(itemId))
                _switchesStates.Add(itemId, state);
            else
                _switchesStates[itemId] = state;
        }

        public bool WasSwitched(string itemId) => _switchesStates.ContainsKey(itemId);

        public bool RestoreSwitchState(string itemId)
        {
            if (_switchesStates.ContainsKey(itemId))
                return _switchesStates[itemId];
            else throw new System.ArgumentException("No such door!");
        }

        public void ClearStates()
        {
            _switchesStates.Clear();
            _removedItems.Clear();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            _trash.Dispose();
        }
    }
}
