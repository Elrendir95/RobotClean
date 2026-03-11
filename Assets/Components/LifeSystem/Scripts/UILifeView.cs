using System;
using Library.References;
using UnityEngine;
using UnityEngine.UI;

namespace Components.LifeSystem
{
    public class UILifeView : MonoBehaviour
    {
        [Header("Life References")]
        [SerializeField, Tooltip("Player Current life references")] private FloatReference _lifeCount;
        [SerializeField, Tooltip("Player Max life references")] private FloatReference _maxLifeCount;
        [Header("UI References")]
        [SerializeField, Tooltip("UI elements references")] private Slider slider;

        private void OnEnable()
        {
            _lifeCount.OnValueChanged.AddListener(OnLifeChanged);
        }

        private void OnDisable()
        {
            _lifeCount.OnValueChanged.RemoveListener(OnLifeChanged);
        }

        private void OnLifeChanged(float currentLife)
        {
            slider.value = currentLife / _maxLifeCount.Value;
        }
    }
}
