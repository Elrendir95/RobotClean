using UnityEngine.SceneManagement;

namespace Components.SceneLoader
{
    public static class SceneLoaderService
    {
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public static void LoadGame()
        {
            SceneManager.LoadScene("Game",  LoadSceneMode.Single);
            SceneManager.LoadScene("Countdown", LoadSceneMode.Additive);
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
