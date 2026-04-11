using System;
using Components.EventSystem;
using Components.StateMachine;
using Components.StateMachine.States;
using Library.Variables;
using UnityEngine;

namespace Components.Collectible
{
    public class ElectronicsComponentsCollector : MonoBehaviour
    {
        [SerializeField, Tooltip("Shared variable that keep tracks of the electronics collected in the current run")]
        private IntVariable inRunElectronicsComponents;

        private void Start()
        {
            Events.OnElectronicsCollected += OnElectronicsCollected;
            Events.OnStateChanged += OnStateChanged;
        }

        private void OnStateChanged(State newState)
        {
            if (newState is not GameOverState) return;

            // Get the Saved Information,
            var save = SaveSystem.SaveService.Load();
            // Update the amount of electronics components
            save.electronicsComponents += inRunElectronicsComponents.Value;
            // Save the new information
            SaveSystem.SaveService.Save(save);
        }

        private void OnDestroy()
        {
            Events.OnElectronicsCollected -= OnElectronicsCollected;
            Events.OnStateChanged -= OnStateChanged;
        }

        private void OnElectronicsCollected(int amount)
        {
            inRunElectronicsComponents.Value += amount;
        }
    }
}
