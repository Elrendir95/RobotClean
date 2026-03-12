using System;
using Components.EventSystem;
using Library.References;
using TMPro;
using UnityEngine;

namespace Components
{
    public class UIDistanceView :  MonoBehaviour
    {
        [SerializeField] private TMP_Text distanceText;
        [SerializeField] private FloatReference distanceReference;

        private void OnEnable()
        {
            distanceReference.OnValueChanged.AddListener(UpdateView);
        }

        private void OnDisable()
        {
            distanceReference.OnValueChanged.RemoveListener(UpdateView);
        }

        private void Start()
        {
            UpdateView(distanceReference.Value);
        }

        private void UpdateView(float newDistance)
        {
            distanceText.text = newDistance.ToString("0");
        }
    }
}
