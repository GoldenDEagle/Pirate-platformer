using System;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model.Data.Properties;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;

        private StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;

        public event Action OnLocaleChanged;

        public string LocaleKey => _localeKey.Value;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }

        private void LoadLocale(string localeToLoad)
        {
            var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = def.GetData();
            _localeKey.Value = localeToLoad;
            OnLocaleChanged?.Invoke();
        }

        public string Localize(string key)
        {
            var localized = _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
            if (_localeKey.Value == "heb")
                localized = Reverse(localized);
            return localized;
        }

        public void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}