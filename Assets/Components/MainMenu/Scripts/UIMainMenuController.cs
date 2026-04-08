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
            _saveData = SaveService.Load();
            highScoreText.text = _saveData.highScore.ToString();
            highScoreWindow.SetActive(_saveData.highScore > 0);
        }

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
