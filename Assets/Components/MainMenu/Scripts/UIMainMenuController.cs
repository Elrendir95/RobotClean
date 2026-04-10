using System;
using System.Collections;
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
        [SerializeField] private float execDelay = 0.4f;

        private SaveData _saveData;

        private void Start()
        {
            // Initialize timeScale to 1, in case of we used Pause menu
            Time.timeScale = 1;
            _saveData = SaveService.Load();
            highScoreText.text = _saveData.highScore.ToString();
            highScoreWindow.SetActive(_saveData.highScore > 0);
        }

        /// <summary>
        /// Coroutine used to delay the scene loader
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IEnumerator ExecuteDelayCoroutine(Action action)
        {
            yield return new WaitForSeconds(execDelay);
            action();
        }

        public void NewGame()
        {
            StartCoroutine(ExecuteDelayCoroutine(SceneLoaderService.LoadGame));
        }

        public void QuitGame()
        {
            StartCoroutine(ExecuteDelayCoroutine(SceneLoaderService.QuitGame));
        }
    }
}
