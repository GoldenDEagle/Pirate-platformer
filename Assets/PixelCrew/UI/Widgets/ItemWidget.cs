using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount itemWithValue)
        {
            var def = DefsFacade.I.Items.Get(itemWithValue.ItemId);
            _icon.sprite = def.Icon;

            _value.text = itemWithValue.Count.ToString();
        }
    }
}