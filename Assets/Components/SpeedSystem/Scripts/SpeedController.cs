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

        private void OnEnable()
        {
            Events.OnLifeCountChanged += OnLifeCountChanged;
            Events.OnStateChanged += OnStateChanged;
        }

        private void OnStateChanged(State newState)
        {
            if (newState is not GameState)
            {
                runtimeSpeed.Value = 0;
                StopCoroutine(SpeedCoroutine());
                return;
            }
            runtimeSpeed.Value = startSpeed.Value;
            StartCoroutine(SpeedCoroutine());
        }

        private void OnDisable()
        {
            Events.OnLifeCountChanged -= OnLifeCountChanged;
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
            while (true)
            {
                yield return new WaitForSeconds(increaseSpeedTime.Value);
                if (runtimeSpeed.Value > maxSpeed.Value) continue;
                runtimeSpeed.Value += increaseSpeedAmount.Value;
                runtimeSpeed.Value = Mathf.Clamp(runtimeSpeed.Value, startSpeed, maxSpeed.Value);
            }
        }
    }
}
