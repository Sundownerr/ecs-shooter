using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddStateMachineExitTimeSystem : ISystem
    {
        private Filter _filter;
        private Stash<Transition> _transition;
        private Stash<TransitionExitTime> _transitionExitTime;
        private Stash<StateMachineExitTime> _stateMachineExitTime;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NeedsActivation, TransitionExitTime>();
            _transition = World.GetStash<Transition>();
            _transitionExitTime = World.GetStash<TransitionExitTime>();
            _stateMachineExitTime = World.GetStash<StateMachineExitTime>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transition = ref _transition.Get(entity);
                ref var transitionExitTime = ref _transitionExitTime.Get(entity);

                // Debug.Log($"ADd exit time {transitionExitTime.Duration}");
                ref var stateMachineExitTime = ref _stateMachineExitTime.Get(transition.StateMachine);
                stateMachineExitTime.Remaining = transitionExitTime.Duration;
            }
        }
    }
}
