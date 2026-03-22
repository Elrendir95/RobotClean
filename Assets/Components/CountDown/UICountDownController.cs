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
        private bool _inCountDown;

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
            if (newState is CountDownState countDownState)
            {
                window.SetActive(true);
                _countDownState = countDownState;
                _inCountDown = true;
                return;
            }
            window.SetActive(false);
            _inCountDown = false;
        }

        private void Update()
        {
            if (!_inCountDown) return;

            countDownText.text = _countDownState.Timer.ToString("0");
        }
    }
}
