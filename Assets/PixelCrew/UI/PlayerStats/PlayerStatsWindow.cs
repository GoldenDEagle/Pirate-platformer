using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.PlayerStats
{
    public class PlayerStatsWindow : AnimatedWindow
    {
        [SerializeField] private Transform _statsContainer;
        [SerializeField] private StatWidget _prefab;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private ItemWidget _price;

        private DataGroup<StatDef, StatWidget> _dataGroup;

        private float _defaultTimeScale;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        protected override void Start()
        {
            base.Start();

            _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _statsContainer);

            GameSession.Instance.StatsModel.InterfaceSelectedStat.Value = DefsFacade.I.Player.Stats[0].Id;
            
            _trash.Retain(GameSession.Instance.StatsModel.Subscribe(OnStatsChanged));
            _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgrade));

            OnStatsChanged();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void OnUpgrade()
        {
            var selected = GameSession.Instance.StatsModel.InterfaceSelectedStat.Value;
            GameSession.Instance.StatsModel.LevelUp(selected);
        }

        private void OnStatsChanged()
        {
            var stats = DefsFacade.I.Player.Stats;
            _dataGroup.SetData(stats);

            var selected = GameSession.Instance.StatsModel.InterfaceSelectedStat.Value;
            var nextLevel = GameSession.Instance.StatsModel.GetCurrentLevel(selected) + 1;
            var def = GameSession.Instance.StatsModel.GetLevelDef(selected, nextLevel);
            _price.SetData(def.Price);

            _price.gameObject.SetActive(def.Price.Count != 0);
            _upgradeButton.gameObject.SetActive(def.Price.Count != 0);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
            Time.timeScale = _defaultTimeScale;
        }
    }
}
