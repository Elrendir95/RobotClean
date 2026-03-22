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
        }
    }
}
