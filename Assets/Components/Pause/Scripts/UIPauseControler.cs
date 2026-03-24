using Components.EventSystem;
using Components.SceneLoader;
using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;

namespace Components.Pause
{
    public class UIPauseControler : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        private void Awake()
        {
            Events.OnStateChanged += OnStateChanged;
            pausePanel.SetActive(false);

        }

        private void OnDestroy()
        {
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State state)
        {
            pausePanel.SetActive(state is PauseState);
        }

        public void ResumeGame()
        {
            Events.OnResumeGame?.Invoke();
        }

        public void MainMenu()
        {
            SceneLoaderService.LoadMainMenu();
        }

        public void QuitGame()
        {
            SceneLoaderService.QuitGame();
        }
    }
}
