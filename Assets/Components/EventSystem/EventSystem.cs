using System;
using UnityEngine;
using Components.AudioSystem;
using Components.StateMachine;

namespace Components.EventSystem
{
    public static class Events
    {
        public static Action<int> OnElectronicsCollected;
        public static Action<float> OnLifeCountChanged;
        public static Action<float> UpdateLife;
        // Player
        public static Action<bool> OnPlayerInvincible;
        public static Action<bool> OnPlayerSlidingDown;
        // Game States Events
        public static Action<State> OnStateChanged;
        public static Action OnResumeGame;
        public static Action<ChunkController> OnChunkSpawned;
        // Audio Events
        public static Action<AudioSO> PlayAudio;
        public static Action<AudioSO, Vector3> PlayAudioAt;
        public static Action StopAllLoops;
        // Skill
        public static Action<string> BuySkill;
        public static Action<int> UpdateElectronicsUI;
    }
}
