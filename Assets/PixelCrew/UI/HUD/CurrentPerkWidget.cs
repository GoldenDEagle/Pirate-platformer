using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD
{
    public class CurrentPerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownFiller;

        public void Set(PerkDef perkDef)
        {
            _icon.sprite = perkDef.Icon;
        }

        private void Update()
        {
            var cooldown = GameSession.Instance.PerksModel.Cooldown;
            _cooldownFiller.fillAmount = cooldown.RemainingTime / cooldown.Value;
        }
    }
}