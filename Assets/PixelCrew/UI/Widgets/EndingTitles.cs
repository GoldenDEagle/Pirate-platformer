using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class EndingTitles : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public void Show()
        {
            _text.LerpAnimated(0, 1, 5, x => _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, x));
        }
    }
}