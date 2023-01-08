using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PixelCrew.UI.Widgets;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils;

namespace PixelCrew.UI.HUD.Dialogs
{
    public class OptionItemWidget : MonoBehaviour, IItemRenderer<OptionData>
    {
        [SerializeField] private Text _label;
        [SerializeField] private SelectOption _onSelect;

        private OptionData _data;

        public void SetData(OptionData data, int index)
        {
            _data = data;
            _label.text = LocalizationManager.I.Localize(data.Text);
        }

        public void OnSelect()
        {
            _onSelect.Invoke(_data);
            _data.OnSelect?.Invoke();
        }

        [Serializable]
        public class SelectOption : UnityEvent<OptionData>
        {
        }
    }
}