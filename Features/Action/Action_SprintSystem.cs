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
    public sealed class Action_SprintSystem : ISystem
    {
        // Stashes for component access
        private Stash<AbilitySprint> _abilitySprint;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<FloatStats> _floatStats;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilitySprint, Active>();

            // Initialize stashes
            _abilitySprint = World.GetStash<AbilitySprint>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _floatStats = World.GetStash<FloatStats>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var sprint = ref _abilitySprint.Get(entity);
                
                Entity targetEntity = default;

                switch (sprint.TargetType) {
                    case TargetType.Self:
                        ref var user = ref _userEntity.Get(entity);
                        targetEntity = user.Entity;
                        break;
                    case TargetType.Target:
                    case TargetType.Other:
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            if (targets.List is {Count: > 0,})
                                targetEntity = targets.List[0]; 
                        }

                        break;
                }

                if (!World.IsDisposed(targetEntity))
                    if (_floatStats.Has(targetEntity)) {
                        ref var floatStats = ref _floatStats.Get(targetEntity);
                        floatStats.Value.AddModifier(Stat.MoveSpeed,
                            StatModifier.Percent(sprint.SprintModifierValue, "SprintAbility"));
                    }
                
                _active.Remove(entity);
            }
        }
    }
}