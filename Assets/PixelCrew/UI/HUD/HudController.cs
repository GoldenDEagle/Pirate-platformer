using UnityEngine;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;

namespace PixelCrew.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnPlayerHealthChanged;

            OnPlayerHealthChanged(_session.Data.Hp.Value, 0);
        }

        private void OnPlayerHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float) newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        public void OnPause()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }

        public void OnTest()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow");
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnPlayerHealthChanged;
        }
    }
}
