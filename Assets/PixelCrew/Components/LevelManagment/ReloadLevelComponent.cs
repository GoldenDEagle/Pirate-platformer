﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;

namespace PixelCrew.Components
{

    public class ReloadLevelComponent : MonoBehaviour
    {
        private Scene startScene;

        private void Awake()
        {
            startScene = SceneManager.GetActiveScene();
        }
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            var scene = SceneManager.GetActiveScene();

            if (scene.name == startScene.name)
            {
                Destroy(session.gameObject);
            }
            else
            {
                session.LoadLastSave();
            }

            SceneManager.LoadScene(scene.name);
        }
    }
}