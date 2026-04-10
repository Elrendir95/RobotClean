using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;
using Components.EventSystem;
using Components.SaveSystem;
using Components.SceneLoader;
using Library.References;
using TMPro;
using UnityEngine.Serialization;

namespace Components.GameOver
{
    public class UIGameOverController : MonoBehaviour
    {
        [FormerlySerializedAs("_gameOverPanel")]
        [SerializeField] private GameObject gameOverPanel;
        [Header("High Score Settings")]
        [SerializeField] private TMP_Text runScoreText;
        [SerializeField] private GameObject newHighScorePanel;
        [SerializeField] private FloatReference score;

        private SaveData _saveData;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
            Events.OnStateChanged += OnStateChanged;
        }

        private void Start()
        {
            _saveData = SaveService.Load();
        }

        private void OnDestroy()
        {
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State newState)
        {
            if (newState is not GameOverState)
            {
                gameOverPanel.SetActive(false);
                return;
            }

            HandleHighScore();
            gameOverPanel.SetActive(true);
        }

        private void HandleHighScore()
        {
            runScoreText.text = score.Value.ToString("0");

            if (score.Value > _saveData.highScore)
            {
                // Display that is the new High Score
                newHighScorePanel.SetActive(true);
                // Save the new HighScore
                _saveData.highScore = Mathf.FloorToInt(score.Value);
                SaveService.Save(_saveData);
            }
            else
            {
                newHighScorePanel.SetActive(false);
            }
        }

        public void RestartGame()
        {
            // Reset the current Score
            score.Value = 0;
            SceneLoaderService.LoadGame();
        }

        public void MainMenu()
        {
            SceneLoaderService.LoadMainMenu();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
