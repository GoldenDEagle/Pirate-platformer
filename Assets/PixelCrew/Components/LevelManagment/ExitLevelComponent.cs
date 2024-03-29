﻿using PixelCrew.Model;
using PixelCrew.UI;
using UnityEngine;

namespace PixelCrew.Components
{

    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            GameSession.Instance.ClearStates();
            GameSession.Instance.Save();

            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}
