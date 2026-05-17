using Core.Initialization;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Core.Managers
{
    public class SceneManager : MonoBehaviour, ISingle
    {
        public void RestartScene()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Time.timeScale = 1f;
            Hub.gameSaver.SaveGame();
            Application.Quit();
        }

        public void ExitToMainMenu()
        {
            StopSceneTime();
            Hub.gameSaver.SaveGame();
            // Load Main Menu Scene
        }

        public void StartSceneTime()
        {
            Time.timeScale = 1f;
        }

        public void StopSceneTime()
        {
            Time.timeScale = 0f;
        }
    }
}
