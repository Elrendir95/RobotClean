using Components.SaveSystem;
using Components.SceneLoader;
using TMPro;
using UnityEngine;

namespace Components.MainMenu
{
    public class UIMainMenuController :  MonoBehaviour
    {
        [SerializeField] private GameObject highScoreWindow;
        [SerializeField] private TMP_Text highScoreText;

        private SaveData _saveData;

        private void Start()
        {
            _saveData = SaveService.Load();
            highScoreText.text = _saveData.highScore.ToString();
            highScoreWindow.SetActive(_saveData.highScore > 0);
        }

        public void NewGame()
        {
            SceneLoaderService.LoadGame();
        }

        public void QuitGame()
        {
            SceneLoaderService.QuitGame();
        }
    }
}
