using System.Collections.Generic;

namespace Components.InputBuffer
{
    public class InputBufferHandler
    {
        private readonly Dictionary<ActionType, BufferedInput> _buffer = new Dictionary<ActionType, BufferedInput>();

        public void AddInput(ActionType actionType, float bufferedTime)
        {
            _buffer[actionType] = new BufferedInput(bufferedTime);
        }

        public void ClearInput()
        {
            _buffer.Clear();
        }

        public bool IsBuffered(ActionType actionType)
        {
            return _buffer.ContainsKey(actionType) && _buffer[actionType].IsActive;
        }

        public void Consume(ActionType actionType)
        {
            _buffer.Remove(actionType);
        }
    }
}
