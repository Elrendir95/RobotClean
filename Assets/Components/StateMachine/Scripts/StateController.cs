using UnityEngine;

namespace Components.StateMachine
{
    public class StateController : MonoBehaviour
    {
        private StateMachine _stateMachine;

        public void Start()
        {
            _stateMachine = new StateMachine();
            var initialState = new States.CountDown(_stateMachine);
            _stateMachine.ChangeState(initialState);
        }
    }
}
