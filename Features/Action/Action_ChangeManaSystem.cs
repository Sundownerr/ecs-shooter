using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_ChangeManaSystem : ISystem
    {
        // Stashes for component access
        private Stash<AbilityChangeMana> _abilityChangeMana;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<FloatStats> _floatStats;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityChangeMana, Active>();

            // Initialize stashes
            _abilityChangeMana = World.GetStash<AbilityChangeMana>();
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
                ref var changeMana = ref _abilityChangeMana.Get(entity);
                ref var userEntity = ref _userEntity.Get(entity);
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                Entity targetEntity = default;

                // Determine target based on TargetType
                switch (changeMana.TargetType) {
                    case TargetType.Self:
                        targetEntity = userEntity.Entity;
                        break;
                    case TargetType.Target:
                    case TargetType.Other:
                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            if (targets.List is {Count: > 0,})
                                targetEntity = targets.List[0]; // Use the first target
                        }

                        break;
                }

                // Modify the FloatStats component
                if (!World.IsDisposed(targetEntity) && _floatStats.Has(targetEntity)) {
                    ref var floatStats = ref _floatStats.Get(targetEntity);

                    var currentMana = floatStats.Value.ValueOf(Stat.Mana);
                    var maxMana = floatStats.Value.ValueOf(Stat.MaxMana);

                    // Change mana (positive value increases, negative decreases)
                    // Ensure mana doesn't go below zero or above max
                    var newMana = Mathf.Clamp(
                        currentMana + changeMana.ManaChange,
                        0,
                        maxMana
                    );

                    floatStats.Value.Set(Stat.Mana, newMana);
                }

                _active.Remove(entity);
            }
        }
    }
}