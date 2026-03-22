using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;
using Components.EventSystem;
using Components.SceneLoader;

namespace Components.GameOver
{
    public class UIGameOverController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;

        private void Awake()
        {
            _gameOverPanel.SetActive(false);
            Events.OnStateChanged += OnStateChanged;
        }

        private void OnDestroy()
        {
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State newState)
        {
            _gameOverPanel.SetActive(newState is GameOverState);
        }

        public void RestartGame()
        {
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
