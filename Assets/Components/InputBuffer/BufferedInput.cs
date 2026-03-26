using UnityEngine;

namespace Components.InputBuffer
{
    public class BufferedInput
    {
        private readonly float _expirationTime;
        public bool IsActive => Time.time <= _expirationTime;

        public BufferedInput(float expirationTime)
        {
            _expirationTime = Time.time + expirationTime;
        }
    }
}
