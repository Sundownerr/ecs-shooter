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
    public sealed class Action_AddFloatStatModifierSystem : ISystem
    {
        private Stash<AbilityAddFloatStatModifier> _abilityAddFloatStatModifier;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<FloatStats> _floatStats;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityAddFloatStatModifier, Active>();

            _abilityAddFloatStatModifier = World.GetStash<AbilityAddFloatStatModifier>();
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
                ref var addModifier = ref _abilityAddFloatStatModifier.Get(entity);
                ref var userEntity = ref _userEntity.Get(entity);
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                Entity targetEntity = default;

                switch (addModifier.TargetType) {
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

                if (!World.IsDisposed(targetEntity) && _floatStats.Has(targetEntity)) {
                    ref var floatStats = ref _floatStats.Get(targetEntity);

                    var modifier = addModifier.ModifierType == ModifierType.Flat
                        ? StatModifier.Flat(addModifier.Value, addModifier.ModifierId)
                        : StatModifier.Percent(addModifier.Value, addModifier.ModifierId);

                    floatStats.Value.AddModifier(addModifier.StatId, modifier);
                }

                _active.Remove(entity);
            }
        }
    }
}