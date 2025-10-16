using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ChangeStateMachineStateSystem : ISystem
    {
        private Filter _filter;
        private Stash<Active> _active;
        private Stash<StateMachine> _stateMachine;
        private Stash<ChangeState> _changeState;
        private Stash<Cancelled> _cancelled;
        private Stash<EnterAction> _enterAction;
        private Stash<NeedsActivation> _needsActivation;
        private Stash<ShouldResetDurations> _shouldResetDurations;
        private Stash<TransitionFrom> _transitionFrom;
        private Stash<ForwardConditions> _forwardConditions;
        private Stash<ForwardConditionsToMeet> _forwardConditionsToMeet;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<StateMachine, ChangeState>();
            _active = World.GetStash<Active>();
            _stateMachine = World.GetStash<StateMachine>();
            _changeState = World.GetStash<ChangeState>();
            _cancelled = World.GetStash<Cancelled>();
            _enterAction = World.GetStash<EnterAction>();
            _needsActivation = World.GetStash<NeedsActivation>();
            _shouldResetDurations = World.GetStash<ShouldResetDurations>();
            _transitionFrom = World.GetStash<TransitionFrom>();
            _forwardConditions = World.GetStash<ForwardConditions>();
            _forwardConditionsToMeet = World.GetStash<ForwardConditionsToMeet>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var stateMachine = ref _stateMachine.Get(entity);
                ref var changeState = ref _changeState.Get(entity);

                if (!stateMachine.PreviousTransition.IsNullOrDisposed())
                    _cancelled.Add(stateMachine.PreviousTransition);

                stateMachine.PreviousTransition = changeState.TransitionEntity;

                foreach (var enterActionEntity in stateMachine.EnterActions)
                {
                    ref var enterAction = ref _enterAction.Get(enterActionEntity);

                    var active = _active.Has(enterActionEntity);
                    var matchingNextState = enterAction.State == changeState.NextState;

                    if (active && !matchingNextState)
                    {
                        _active.Remove(enterActionEntity);
                        _cancelled.Add(enterActionEntity);
                        // Debug.Log($"CANCEL: {enterAction.State}");
                    }

                    if (matchingNextState && !active)
                    {
                        _active.Add(enterActionEntity);
                        _needsActivation.Add(enterActionEntity);
                        _shouldResetDurations.Add(enterActionEntity);

                        // Debug.Log($"ENTER: {enterAction.State}");
                    }
                }

                foreach (var transition in stateMachine.Transitions)
                {
                    ref var transitionFrom = ref _transitionFrom.Get(transition);
                    var transitionActive = _active.Has(transition);
                    var matchingNextTransition = transitionFrom.State == changeState.NextState;

                    if (_forwardConditions.Has(transition))
                    {
                        ref var forwardConditions = ref _forwardConditions.Get(transition);

                        ref var conditionsToMeet = ref _forwardConditionsToMeet.Get(transition);
                        conditionsToMeet.Remaining = forwardConditions.List.Length;

                        if (transitionActive && !matchingNextTransition)
                            foreach (var condition in forwardConditions.List)
                                _active.Remove(condition);

                        if (matchingNextTransition && !transitionActive)
                            foreach (var condition in forwardConditions.List)
                                _active.Add(condition);
                    }

                    if (transitionActive && !matchingNextTransition)
                        _active.Remove(transition);

                    if (matchingNextTransition && !transitionActive)
                        _active.Add(transition);
                }

                // Debug.Log($"Change state to {changeState.NextState}");
            }
        }
    }
}
