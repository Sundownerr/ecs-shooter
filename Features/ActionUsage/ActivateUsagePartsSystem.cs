using System;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ActivateUsagePartsSystem : ISystem
    {
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<Duration> _duration;
        private Stash<UsageConfigs> _usageConfigs;
        private Stash<UsageParts> _usageParts;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Active, Duration, UsageProgressPart>();
            _parentEntity = World.GetStash<ParentEntity>();
            _duration = World.GetStash<Duration>();
            _usageConfigs = World.GetStash<UsageConfigs>();
            _usageParts = World.GetStash<UsageParts>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parent = ref _parentEntity.Get(entity);
                ref var duration = ref _duration.Get(entity);

                ref var usageConfigs = ref _usageConfigs.Get(parent.Entity);
                ref var usageParts = ref _usageParts.Get(parent.Entity);

                foreach (var config in usageConfigs.List)
                {
                    if (usageParts.UsagePartActivated[config])
                        continue;

                    bool shouldActivateUsagePart;

                    switch (config.Type)
                    {
                        case AbilityUsageEntry.Use.Time:
                            shouldActivateUsagePart = duration.Elapsed >= config.Value;
                            break;

                        case AbilityUsageEntry.Use.Percent:
                            shouldActivateUsagePart = duration.PercentElapsed >= config.Value;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (!shouldActivateUsagePart)
                        continue;

                    usageParts.UsagePartActivated[config] = true;

                    foreach (var action in usageParts.UsagePartActions[config])
                        _active.Add(action);

                    // Debug.Log(
                    // $"Activating {config.Value}, Elapsed: {duration.Elapsed:F1}, Percent: {duration.PercentElapsed * 100f:F0}%");
                }
            }
        }
    }
}
