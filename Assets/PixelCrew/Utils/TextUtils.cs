using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;
using PixelCrew.Utils;

namespace PixelCrew.Utils
{
    public static class TextUtils
    {
        public static void LocalizeFont(this Text text)
        {
            var localeKey = LocalizationManager.I.LocaleKey;

            var font = Resources.Load<Font>($"Fonts/{localeKey}");

            text.font = font;
        }
    }
}