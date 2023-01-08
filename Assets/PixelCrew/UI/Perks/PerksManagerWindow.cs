using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;
using PixelCrew.Utils;

namespace PixelCrew.UI.Perks
{
    public class PerksManagerWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _info;
        [SerializeField] Transform _perksContainer;

        private PredifinedDataGroup<PerkDef, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new PredifinedDataGroup<PerkDef, PerkWidget>(_perksContainer);

            _trash.Retain(GameSession.Instance.PerksModel.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));

            OnPerksChanged();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = GameSession.Instance.PerksModel.InterfaceSelection.Value;

            _useButton.gameObject.SetActive(GameSession.Instance.PerksModel.IsUnlocked(selected));
            _useButton.interactable = GameSession.Instance.PerksModel.Used != selected;

            _buyButton.gameObject.SetActive(!GameSession.Instance.PerksModel.IsUnlocked(selected));
            _buyButton.interactable = GameSession.Instance.PerksModel.CanBuy(selected);

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _info.text = LocalizationManager.I.Localize(def.Info);
        }

        private void OnBuy()
        {
            var selected = GameSession.Instance.PerksModel.InterfaceSelection.Value;
            GameSession.Instance.PerksModel.Unlock(selected);
        }

        private void OnUse()
        {
            var selected = GameSession.Instance.PerksModel.InterfaceSelection.Value;
            GameSession.Instance.PerksModel.SelectPerk(selected);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
            Time.timeScale = _defaultTimeScale;
        }
    }
}
