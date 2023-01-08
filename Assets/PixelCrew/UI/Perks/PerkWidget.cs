using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isLocked;
        [SerializeField] private GameObject _isSelected;
        [SerializeField] private GameObject _isUsed;

        private PerkDef _data;

        private void Start()
        {
            UpdateView();
        }

        public void SetData(PerkDef data, int index)
        {
            _data = data;

            if (GameSession.Instance != null)
                UpdateView();
        }

        public void OnSelect()
        {
            GameSession.Instance.PerksModel.InterfaceSelection.Value = _data.Id;
        }

        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _isUsed.SetActive(GameSession.Instance.PerksModel.IsUsed(_data.Id));
            _isSelected.SetActive(GameSession.Instance.PerksModel.InterfaceSelection.Value == _data.Id);
            _isLocked.SetActive(!GameSession.Instance.PerksModel.IsUnlocked(_data.Id));
        }
    }
}