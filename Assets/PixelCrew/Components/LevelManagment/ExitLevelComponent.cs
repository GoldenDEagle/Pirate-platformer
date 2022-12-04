using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;

namespace PixelCrew.Components
{

    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.Save();
            SceneManager.LoadScene(_sceneName);
        }
    }
}
