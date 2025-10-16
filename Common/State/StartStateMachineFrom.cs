using System.Collections.Generic;
using VContainer.Unity;

namespace Game.Common
{
    public class StartStateMachineFrom<TState> : IStartable, ITickable, IFixedTickable, ILateTickable
        where TState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IEnumerable<IState> _states;

        public StartStateMachineFrom(StateMachine stateMachine, IEnumerable<IState> states)
        {
            _stateMachine = stateMachine;
            _states = states;
        }

        public void FixedTick() => _stateMachine.FixedTick();

        public void LateTick() => _stateMachine.LateTick();

        public void Start()
        {
            foreach (var state in _states)
                _stateMachine.AddState(state);

            _stateMachine.ChangeState<TState>();
        }

        public void Tick() => _stateMachine.Tick();
    }
}