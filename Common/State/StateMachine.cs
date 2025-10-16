using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new();
        private IState _currentState;

        public void FixedTick()
        {
            if (_currentState is IFixedTickableState state)
                state.FixedTick();
        }

        public void LateTick()
        {
            if (_currentState is ILateTickableState state)
                state.LateTick();
        }

        public void Tick()
        {
            if (_currentState is ITickableState state)
                state.Tick();
        }

        public void AddState<T>(T state) where T : IState
        {
            var type = state.GetType();

            if (_states.ContainsKey(type))
                return;

            _states.Add(type, state);
        }

        public void ChangeState<T>() where T : IState
        {
            switch (_currentState) {
                case IExitState exitState:
                    exitState.Exit();
                    break;
                case IExitStateAsync exitStateAsync:
                    exitStateAsync.Exit();
                    break;
            }

            Debug.Log($"{_currentState?.GetType().Name} -> {typeof(T).Name}");
            
            _currentState = _states[typeof(T)];
            
            switch (_currentState) {
                case IEnterState enterState:
                    enterState.Enter();
                    break;
                case IEnterStateAsync enterStateAsync:
                    enterStateAsync.Enter();
                    break;
            }
            
        }
        
    }
}