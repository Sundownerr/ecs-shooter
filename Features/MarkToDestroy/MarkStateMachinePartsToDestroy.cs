using Game.Components;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkStateMachinePartsToDestroy : ISystem
    {
        private Filter _filter;
        private Stash<StateMachine> _stateMachine;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<StateMachine, WillBeDestroyed>();
            _stateMachine = World.GetStash<StateMachine>();
            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var stateMachine = ref _stateMachine.Get(entity);

                foreach (var transitionEntity in stateMachine.Transitions)
                    _willBeDestroyed.Add(transitionEntity);

                foreach (var enterAction in stateMachine.EnterActions)
                    _willBeDestroyed.Add(enterAction);
            }
        }
    }
}
