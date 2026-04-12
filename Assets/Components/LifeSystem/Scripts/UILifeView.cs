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
        [SerializeField] private float lifeWarningThreshold = 30f;
        [SerializeField] private Color lifeWarningColor;
        [Header("UI References")]
        [SerializeField, Tooltip("UI elements references")] private Slider slider;

        private Image _fillImage;
        private Color _lifeColor;

        void Awake()
        {
            _fillImage = slider.fillRect.gameObject.GetComponent<Image>();
            _lifeColor = _fillImage.color;
            _lifeCount.OnValueChanged.AddListener(OnLifeChanged);
            // Make sure we update the Lifebar at awake
            OnLifeChanged(_lifeCount.Value);
        }

        private void OnDestroy()
        {
            _lifeCount.OnValueChanged.RemoveListener(OnLifeChanged);
        }

        private void OnLifeChanged(float currentLife)
        {
            slider.value = currentLife / _maxLifeCount.Value;
            _fillImage.color = (currentLife < lifeWarningThreshold) ? lifeWarningColor : _lifeColor;
        }
    }
}
