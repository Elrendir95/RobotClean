using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
using TMPro;
using UnityEngine;

namespace Components.CountDown
{
    public class UICountDownController : MonoBehaviour
    {
        [SerializeField] private GameObject window;
        [SerializeField] TMP_Text countDownText;

        private CountDownState _countDownState;

        private void OnEnable()
        {
            Events.OnStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State newState)
        {
            Debug.Log("UICountDownController:Countdown state changed to " + newState);
            if (newState is CountDownState countDownState)
            {
                window.SetActive(true);
                _countDownState = countDownState;
                return;
            }
            window.SetActive(false);
        }

        private void Update()
        {
            if (!window.activeSelf) return;

            countDownText.text = _countDownState.Timer.ToString("0");
        }
    }
}
