using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ChangePreviousTransitionSystem : ISystem
    {
        private Filter _filter;
        private Stash<Cancelled> _cancelled;
        private Stash<StateMachine> _stateMachine;
        private Stash<ChangeState> _changeState;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<StateMachine, ChangeState>();
            _cancelled = World.GetStash<Cancelled>();
            _stateMachine = World.GetStash<StateMachine>();
            _changeState = World.GetStash<ChangeState>();
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
            }
        }
    }
}
