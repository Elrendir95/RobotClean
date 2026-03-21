namespace Components.StateMachine
{
    public abstract class State
    {
        protected StateMachine StateMachine;

        protected State(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
