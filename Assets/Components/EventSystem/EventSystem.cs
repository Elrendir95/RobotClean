using System;

namespace Components.EventSystem
{
    public static class Events
    {
        public static Action<float> OnLifeCountChanged;
        public static Action<float> UpdateLife;
        public static Action<bool> OnPlayerInvincible;
    }
}
