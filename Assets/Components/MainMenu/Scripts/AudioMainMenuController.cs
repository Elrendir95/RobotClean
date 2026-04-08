using System;
using Components.AudioSystem;
using Components.EventSystem;
using UnityEngine;

namespace Components.MainMenu
{
    public class AudioMainMenuController : MonoBehaviour
    {
        [SerializeField] private AudioSO clickSound;
        [SerializeField] private AudioSO musicLoop;

        private void Awake()
        {
            if (!musicLoop.config.loop)
            {
                Debug.LogWarning("Music not a loop!");
            }

            if (clickSound.config.loop)
            {
                Debug.LogWarning("Sound is a loop!");
            }
        }

        public void Start()
        {
            Events.PlayAudio?.Invoke(musicLoop);
        }

        public void PlayClickSound()
        {
            Events.PlayAudio?.Invoke(clickSound);
        }
    }
}
