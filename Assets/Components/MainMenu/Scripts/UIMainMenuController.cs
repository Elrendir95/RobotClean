using Components.SceneLoader;
using UnityEngine;

namespace Components.MainMenu
{
    public class UIMainMenuController :  MonoBehaviour
    {
        public void NewGame()
        {
            SceneLoaderService.LoadGame();
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
