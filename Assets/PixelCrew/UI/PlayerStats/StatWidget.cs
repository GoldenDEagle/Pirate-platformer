﻿using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using PixelCrew.Utils;

namespace PixelCrew.UI.PlayerStats
{
    public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _currentValue;
        [SerializeField] private Text _increaseValue;
        [SerializeField] private ProgressBarWidget _progress;
        [SerializeField] private GameObject _selector;

        private StatDef _data;

        private void Start()
        {
            UpdateView();
        }

        public void SetData(StatDef data, int index)
        {
            _data = data;
            if (GameSession.Instance != null)
                UpdateView();
        }

        private void UpdateView()
        {
            var statsModel = GameSession.Instance.StatsModel;

            _icon.sprite = _data.Icon;
            _name.LocalizeFont();
            _name.text = LocalizationManager.I.Localize(_data.Name);

            _currentValue.text = statsModel.GetValue(_data.Id).ToString(CultureInfo.InvariantCulture);

            var currentLevel = statsModel.GetCurrentLevel(_data.Id);
            var nextLevel = currentLevel + 1;
            var increaseValue = statsModel.GetValue(_data.Id, nextLevel) - statsModel.GetValue(_data.Id, currentLevel);
            _increaseValue.text = $"+ {increaseValue}";
            _increaseValue.gameObject.SetActive(increaseValue > 0);

            var maxLevel = DefsFacade.I.Player.GetStat(_data.Id).Levels.Length - 1;
            _progress.SetProgress(currentLevel / (float) maxLevel);

            _selector.SetActive(statsModel.InterfaceSelectedStat.Value == _data.Id);
        }

        public void OnSelect()
        {
            GameSession.Instance.StatsModel.InterfaceSelectedStat.Value = _data.Id;
        }
    }
}