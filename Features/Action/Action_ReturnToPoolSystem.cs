using System.Collections.Generic;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_ReturnToPoolSystem : ISystem
    {
        private readonly List<AbilityProvider> _abilityProviders = new();
        private Filter _filter;
        private readonly List<StateMachineProvider> _stateMachineProviders = new();

        // Stashes for component access
        private Stash<ReturnToPool> _returnToPool;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ReturnToPool, Active>();

            // Initialize stashes
            _returnToPool = World.GetStash<ReturnToPool>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var returnToPool = ref _returnToPool.Get(entity);

                GameObjectPool.Return(returnToPool.Config.Target);

                returnToPool.Config.Target.GetComponentsInChildren(_abilityProviders);
                foreach (var abilityProvider in _abilityProviders)
                    abilityProvider.ResetState();

                returnToPool.Config.Target.GetComponentsInChildren(_stateMachineProviders);
                foreach (var stateMachineProvider in _stateMachineProviders)
                    stateMachineProvider.ResetState();

                _active.Remove(entity);
            }
        }
    }
}
