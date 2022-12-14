using System;
using PixelCrew.Utils.Disposables;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils;

namespace PixelCrew.Model.Data
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        public string Used => _data.Perks.Used.Value;
        public readonly Cooldown Cooldown = new Cooldown();

        public bool IsMegaThrowSupported => _data.Perks.Used.Value == "mega-throw" && Cooldown.IsReady;
        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "double-jump" && Cooldown.IsReady;

        public event Action OnChanged;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var haveEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (haveEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);

                OnChanged?.Invoke();
            }
        }

        public void SelectPerk(string id)
        {
            var perkDef = DefsFacade.I.Perks.Get(id);
            Cooldown.Value = perkDef.Cooldown;
            _data.Perks.Used.Value = id;
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}