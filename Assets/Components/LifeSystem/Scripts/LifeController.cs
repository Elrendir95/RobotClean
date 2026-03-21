using System.Collections;
using Library.References;
using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
using UnityEngine;

namespace Components.LifeSystem
{
    public class LifeController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Player Current life references")] private FloatReference lifeCount;
        [SerializeField, Tooltip("Player Max life references")] private FloatReference maxLifeCount;
        [SerializeField, Tooltip("Life decrease rate in seconds")] private FloatReference lifeDecreaseRate;
        [SerializeField, Tooltip("Life decrease amount")] private FloatReference lifeDecreaseAmount;

        private bool _isInvincible;

        private void OnEnable()
        {
            Events.UpdateLife += UpdateLife;
            Events.OnPlayerInvincible += HandleOnPlayerInvincible;
            Events.OnStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
           Events.UpdateLife -= UpdateLife;
           Events.OnPlayerInvincible -= HandleOnPlayerInvincible;
           Events.OnStateChanged -= OnStateChanged;
        }

        private void Start()
        {
            lifeCount.Value = maxLifeCount.Value;
        }

        private void HandleOnPlayerInvincible(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }

        private void OnStateChanged(State newState)
        {
            if (newState is not GameState)
            {
                StopCoroutine(LifeDecreaseCoroutine());
                return;
            }
            StartCoroutine(LifeDecreaseCoroutine());
        }

        private void UpdateLife(float lifeAmount)
        {
            lifeCount.Value = Mathf.Clamp(lifeCount.Value + lifeAmount, 0, maxLifeCount.Value);
            Events.OnLifeCountChanged?.Invoke(lifeCount.Value);
        }

        IEnumerator LifeDecreaseCoroutine()
        {
            while (lifeCount.Value > 0)
            {
                yield return new WaitForSeconds(lifeDecreaseRate.Value);

                if (_isInvincible) continue;

                UpdateLife(-lifeDecreaseAmount.Value);
            }
        }
    }
}
