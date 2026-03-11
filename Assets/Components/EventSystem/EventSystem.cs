using System;

namespace Components.EventSystem
{
    public static class Events
    {
        public static Action<int> OnLifeCountChanged;
        public static Action<float> UpdateLife;
    }
}
