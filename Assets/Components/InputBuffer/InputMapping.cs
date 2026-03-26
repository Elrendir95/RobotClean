using System;
using UnityEngine.InputSystem;

namespace Components.InputBuffer
{
    [Serializable]
    public class InputMapping
    {
        public ActionType type;
        public InputActionReference inputActionReference;
        public float bufferingTime;
    }
}
