using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ItemWidget : MonoBehaviour, IItemRenderer<ItemWithCount>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount itemWithValue, int index = -1)
        {
            var def = DefsFacade.I.Items.Get(itemWithValue.ItemId);
            _icon.sprite = def.Icon;

            _value.text = itemWithValue.Count.ToString();
        }
    }
}