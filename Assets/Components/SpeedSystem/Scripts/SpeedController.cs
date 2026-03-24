using System.Collections;
using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
using Library.References;
using UnityEngine;

namespace Components
{
    public class SpeedController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FloatReference startSpeed;
        [SerializeField] private FloatReference maxSpeed;
        [SerializeField] private FloatReference runtimeSpeed;
        [SerializeField] private FloatReference increaseSpeedTime;
        [SerializeField] private FloatReference increaseSpeedAmount;

        // Use to save current speed to be resumed at after a state changed (ie : PauseState)
        private float _resumeSpeed;
        private Coroutine _coroutine;
        private GameState _gameState;

        private void Awake()
        {
            Events.OnLifeCountChanged += OnLifeCountChanged;
            Events.OnStateChanged += OnStateChanged;
            _resumeSpeed = startSpeed.Value;
        }

        private void OnStateChanged(State newState)
        {
            if (newState is not GameState gameState)
            {
                _resumeSpeed = runtimeSpeed.Value > 0 ? runtimeSpeed.Value : startSpeed.Value;
                runtimeSpeed.Value = 0;
                if (_coroutine != null)  StopCoroutine(_coroutine);
                return;
            }
            _gameState = gameState;
            runtimeSpeed.Value = _resumeSpeed;
            _coroutine = StartCoroutine(SpeedCoroutine());
        }

        private void OnDestroy()
        {
            Events.OnLifeCountChanged -= OnLifeCountChanged;
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnLifeCountChanged(float currentLife)
        {
            if (currentLife > 0) return;
            runtimeSpeed.Value = 0;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            runtimeSpeed.Value = 0;
        }

        IEnumerator SpeedCoroutine()
        {
            float delay = increaseSpeedTime.Value - _gameState.Timer % 15;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                if (runtimeSpeed.Value > maxSpeed.Value) continue;
                runtimeSpeed.Value += increaseSpeedAmount.Value;
                runtimeSpeed.Value = Mathf.Clamp(runtimeSpeed.Value, startSpeed, maxSpeed.Value);
                delay = increaseSpeedTime.Value;
            }
        }
    }
}
