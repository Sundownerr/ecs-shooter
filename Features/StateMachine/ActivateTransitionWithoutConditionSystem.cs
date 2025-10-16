using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ActivateTransitionWithoutConditionSystem : ISystem
    {
        private Filter _filter;
        private Stash<Transition> _transition;
        private Stash<StateMachineExitTime> _stateMachineExitTime;
        private Stash<TransitionTo> _transitionTo;
        private Stash<ChangeState> _changeState;
        private Stash<ShouldResetDurations> _shouldResetDurations;
        private Stash<NeedsActivation> _needsActivation;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Transition, Active>().Without<ForwardConditions>().Build();
            _transition = World.GetStash<Transition>();
            _stateMachineExitTime = World.GetStash<StateMachineExitTime>();
            _transitionTo = World.GetStash<TransitionTo>();
            _changeState = World.GetStash<ChangeState>();
            _shouldResetDurations = World.GetStash<ShouldResetDurations>();
            _needsActivation = World.GetStash<NeedsActivation>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transition = ref _transition.Get(entity);

                if (_changeState.Has(transition.StateMachine))
                    continue;

                ref var stateMachineExitTime = ref _stateMachineExitTime.Get(transition.StateMachine);
                if (stateMachineExitTime.Remaining > 0)
                    continue;

                ref var transitionTo = ref _transitionTo.Get(entity);

                // ref var transitionFrom = ref entity.GetComponent<TransitionFrom>();
                // Debug.Log($"Transition from {transitionFrom.State} to {transitionTo.State}");

                ref var changeState = ref _changeState.Add(transition.StateMachine);
                changeState.NextState = transitionTo.State;
                changeState.TransitionEntity = entity;

                _shouldResetDurations.Add(entity);
                _needsActivation.Add(entity);
            }
        }
    }
}
