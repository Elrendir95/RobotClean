using System;
using Library.Variables;
using TMPro;
using UnityEngine;

namespace Components.Collectible
{
    public class UIElectronicsComponents : MonoBehaviour
    {
        [SerializeField] private TMP_Text electronicsText;
        [SerializeField] private IntVariable electronicsCount;

        private void Start()
        {
            electronicsCount.onValueChanged.AddListener(OnElectronicsCountChanged);
            // Manualy call OnElectronicsCountChanged to update at the start the Text
            OnElectronicsCountChanged(electronicsCount.Value);
        }

        private void OnDestroy()
        {
            electronicsCount.onValueChanged.RemoveListener(OnElectronicsCountChanged);
        }

        private void OnElectronicsCountChanged(int newValue)
        {
            electronicsText.text = newValue.ToString("0");
        }
    }
}
