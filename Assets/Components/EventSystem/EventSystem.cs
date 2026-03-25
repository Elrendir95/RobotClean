using System;
using Components.StateMachine;

namespace Components.EventSystem
{
    public static class Events
    {
        public static Action<float> OnLifeCountChanged;
        public static Action<float> UpdateLife;
        public static Action<bool> OnPlayerInvincible;
        public static Action<bool> OnPlayerSlidingDown;
        public static Action<State> OnStateChanged;
        public static Action OnResumeGame;
        public static Action<ChunkController> OnChunkSpawned;
    }
}
