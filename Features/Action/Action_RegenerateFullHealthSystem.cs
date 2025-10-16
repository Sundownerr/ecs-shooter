using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_RegenerateFullHealthSystem : ISystem
    {
        // Stashes for component access
        private Stash<AbilityRegenerateFullHealth> _abilityRegenerateFullHealth;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<Health> _health;
        private Stash<MaxHealth> _maxHealth;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityRegenerateFullHealth, Active>();

            // Initialize stashes
            _abilityRegenerateFullHealth = World.GetStash<AbilityRegenerateFullHealth>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _health = World.GetStash<Health>();
            _maxHealth = World.GetStash<MaxHealth>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var regenerateHealth = ref _abilityRegenerateFullHealth.Get(entity);
                ref var userEntity = ref _userEntity.Get(entity);
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                Entity targetEntity = default;

                switch (regenerateHealth.TargetType) {
                    case TargetType.Self:
                        targetEntity = userEntity.Entity;
                        break;
                    case TargetType.Target:
                    case TargetType.Other:
                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            if (targets.List is {Count: > 0,})
                                targetEntity = targets.List[0];
                        }

                        break;
                }

                if (!World.IsDisposed(targetEntity) && _health.Has(targetEntity) && _maxHealth.Has(targetEntity)) {
                    ref var health = ref _health.Get(targetEntity);
                    ref var maxHealth = ref _maxHealth.Get(targetEntity);

                    health.Value = maxHealth.Value;
                }

                _active.Remove(entity);
            }
        }
    }
}