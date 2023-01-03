using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.Dialogs
{
    public class DialogContent : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _continue;

        public Text Text => _text;

        public void TrySetIcon(Sprite icon)
        {
            if (_icon != null)
                _icon.sprite = icon;
        }

        //private void Update()
        //{
        //    if (Keyboard.current.anyKey.wasReleasedThisFrame)
        //        _continue.onClick?.Invoke();
        //}
    }
}