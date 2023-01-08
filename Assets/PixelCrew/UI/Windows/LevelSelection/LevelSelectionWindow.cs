using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.LevelSelection
{
    public class LevelSelectionWindow : AnimatedWindow
    {
        private Action _closeAction;

        public void OnTutorial()
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


        public void LoadLevel(string levelId)
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();

                loader.LoadLevel(levelId);
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
