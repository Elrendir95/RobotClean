using System.Collections.Generic;

namespace Components.InputBuffer
{
    public class InputBufferHandler
    {
        private readonly Dictionary<ActionType, BufferedInput> _buffer = new();

        /// <summary>
        /// Add an input in the buffer for "bufferedTime"
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="bufferedTime"></param>
        public void AddInput(ActionType actionType, float bufferedTime)
        {
            _buffer[actionType] = new BufferedInput(bufferedTime);
        }

        /// <summary>
        /// Clear all buffered inputs
        /// </summary>
        public void ClearInput()
        {
            _buffer.Clear();
        }

        /// <summary>
        /// Is the action currently buffered and still active
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public bool IsBuffered(ActionType actionType)
        {
            return _buffer.ContainsKey(actionType) && _buffer[actionType].IsActive;
        }

        /// <summary>
        /// Consume the action from the buffer
        /// </summary>
        /// <param name="actionType"></param>
        public void Consume(ActionType actionType)
        {
            _buffer.Remove(actionType);
        }
    }
}
