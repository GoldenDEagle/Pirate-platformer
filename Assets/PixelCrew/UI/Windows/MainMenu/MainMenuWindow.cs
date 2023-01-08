using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Utils;
using PixelCrew.Model;

namespace PixelCrew.UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        protected override void Start()
        {
            base.Start();
            if (GameSession.Instance != null)
                Destroy(GameSession.Instance.gameObject);
        }

        public void OnShowLevels()
        {
            WindowUtils.CreateWindow("UI/LevelSelectionWindow");
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
#if MOBILE_BUILD
                loader.LoadLevel("TutorialLevelMobile");
#else
                loader.LoadLevel("TutorialLevel");
#endif
            };
            Close();
        }

        public void OnShowLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}
