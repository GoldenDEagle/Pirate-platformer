using UnityEngine;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private CurrentPerkWidget _currentPerk;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _trash.Retain(GameSession.Instance.Data.Hp.SubscribeAndInvoke(OnPlayerHealthChanged));
            _trash.Retain(GameSession.Instance.PerksModel.Subscribe(OnPerkChanged));

            OnPerkChanged();
        }

        private void OnPlayerHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = GameSession.Instance.StatsModel.GetValue(StatId.Hp);
            var value = (float) newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnPerkChanged()
        {
            var usedPerkId = GameSession.Instance.PerksModel.Used;
            var hasPerk = !string.IsNullOrEmpty(usedPerkId);
            if (hasPerk)
            {
                var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
                _currentPerk.Set(perkDef);
            }

            _currentPerk.gameObject.SetActive(hasPerk);
        }

        public void OnPause()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }

        //public void OnTest()
        //{
        //    WindowUtils.CreateWindow("UI/PlayerStatsWindow");
        //}

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
