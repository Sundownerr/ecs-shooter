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
    public sealed class ActivateStateNpcActionsSystem : ISystem
    {
        private Filter _filter;
        private Stash<TransitionNpcActions> _transitionNpcActions;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NeedsActivation, TransitionNpcActions>();
            _transitionNpcActions = World.GetStash<TransitionNpcActions>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var actions = ref _transitionNpcActions.Get(entity);

                foreach (var action in actions.List)
                    _active.Add(action);
            }
        }
    }
}
