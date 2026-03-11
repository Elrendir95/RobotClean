using System.Collections;
using Library.References;
using Components.EventSystem;
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

        private void OnEnable()
        {
            Events.UpdateLife += UpdateLife;
        }

        private void OnDisable()
        {
           Events.UpdateLife -= UpdateLife;
        }

        private void UpdateLife(float lifeAmount)
        {
            lifeCount.Value = Mathf.Clamp(lifeCount.Value + lifeAmount, 0, maxLifeCount.Value);
        }

        private void Start()
        {
            lifeCount.Value = maxLifeCount.Value;
            StartCoroutine(LifeDecreaseCoroutine());
        }

        IEnumerator LifeDecreaseCoroutine()
        {
            while (lifeCount.Value > 0)
            {
                yield return new WaitForSeconds(lifeDecreaseRate.Value);
                lifeCount.Value -= lifeDecreaseAmount.Value;
                Events.OnLifeCountChanged?.Invoke(lifeCount.Value);
            }
        }
    }
}
