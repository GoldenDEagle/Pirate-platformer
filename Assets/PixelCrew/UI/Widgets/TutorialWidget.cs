using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class TutorialWidget : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private Image _movement;
        [SerializeField] private Image _interaction;
        [SerializeField] private Image _inventoryUse;
        [SerializeField] private Image _inventorySwitch;
        [SerializeField] private Image _attack;

        //private Func<float, Color, Color> set = SetAlpha;

        //private static Color SetAlpha(float x, Color basic)
        //{
        //    var result = new Color(basic.r, basic.g, basic.b, x);
        //    return result;
        //}

        public void ShowElement(string element)
        {
            switch (element)
            {
                case "movement":
                    _movement.LerpAnimated(0, 1, 1, x => _movement.color = new Color(_movement.color.r, _movement.color.g, _movement.color.b, x));
                    break;
                case "interact":
                    _interaction.LerpAnimated(0, 1, 1, x => _interaction.color = new Color(_interaction.color.r, _interaction.color.g, _interaction.color.b, x));
                    break;
                case "use":
                    _inventoryUse.LerpAnimated(0, 1, 1, x => _inventoryUse.color = new Color(_inventoryUse.color.r, _inventoryUse.color.g, _inventoryUse.color.b, x));
                    break;
                case "switch":
                    _inventorySwitch.LerpAnimated(0, 1, 1, x => _inventorySwitch.color = new Color(_inventorySwitch.color.r, _inventorySwitch.color.g, _inventorySwitch.color.b, x));
                    break;
                case "attack":
                    _attack.LerpAnimated(0, 1, 1, x => _attack.color = new Color(_attack.color.r, _attack.color.g, _attack.color.b, x));
                    break;
                default:
                    throw new ArgumentException("No such tutorial!");
            }
        }

        public void HideElement(string element)
        {
            switch (element)
            {
                case "movement":
                    _movement.LerpAnimated(1, 0, 1, x => _movement.color = new Color(_movement.color.r, _movement.color.g, _movement.color.b, x));
                    break;
                case "interact":
                    _interaction.LerpAnimated(1, 0, 1, x => _interaction.color = new Color(_interaction.color.r, _interaction.color.g, _interaction.color.b, x));
                    break;
                case "use":
                    _inventoryUse.LerpAnimated(1, 0, 1, x => _inventoryUse.color = new Color(_inventoryUse.color.r, _inventoryUse.color.g, _inventoryUse.color.b, x));
                    break;
                case "switch":
                    _inventorySwitch.LerpAnimated(1, 0, 1, x => _inventorySwitch.color = new Color(_inventorySwitch.color.r, _inventorySwitch.color.g, _inventorySwitch.color.b, x));
                    break;
                case "attack":
                    _attack.LerpAnimated(1, 0, 1, x => _attack.color = new Color(_attack.color.r, _attack.color.g, _attack.color.b, x));
                    break;
                default:
                    throw new ArgumentException("No such tutorial!");
            }
        }
    }
}