using Components.EventSystem;

namespace Components.StateMachine
{
    public class StateMachine
    {
        private State _currentState;

        public void ChangeState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
            Events.OnStateChanged?.Invoke(_currentState);
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}
