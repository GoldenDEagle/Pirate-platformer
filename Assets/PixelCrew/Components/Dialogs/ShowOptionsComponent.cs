﻿using UnityEngine;
using PixelCrew.UI.HUD.Dialogs;

namespace PixelCrew.Components.Dialogs
{
    public class ShowOptionsComponent : MonoBehaviour
    {
        [SerializeField] private OptionDialogData _data;

        private OptionDialogController _dialogBox;

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<OptionDialogController>();

            _dialogBox.Show(_data);
        }
    }
}