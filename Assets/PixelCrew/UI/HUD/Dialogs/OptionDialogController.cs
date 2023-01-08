﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PixelCrew.UI.Widgets;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils;

namespace PixelCrew.UI.HUD.Dialogs
{
    public class OptionDialogController : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Text _contentText;
        [SerializeField] private Transform _optionsContainer;
        [SerializeField] private OptionItemWidget _prefab;

        private DataGroup<OptionData, OptionItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<OptionData, OptionItemWidget>(_prefab, _optionsContainer);
        }

        public void OnOptionSelected(OptionData selectedOption)
        {
            //selectedOption.OnSelect.Invoke();
            _content.SetActive(false);
        }

        public void Show(OptionDialogData data)
        {
            _contentText.LocalizeFont();
            _content.SetActive(true);
            _contentText.text = LocalizationManager.I.Localize(data.DialogText);

            _dataGroup.SetData(data.Options);
        }
    }

    [Serializable]
    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }

    [Serializable]
    public class OptionData
    {
        public string Text;
        public UnityEvent OnSelect;
    }
}