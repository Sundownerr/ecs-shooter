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
    public sealed class UpdateDurationSystem : ISystem
    {
        private Stash<Cancelled> _cancelled;
        private Stash<DoNotDeactivateWhenParentChannelEnds> _doNotDeactivateWhenParentChannelEnds;
        private Stash<Duration> _duration;
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<ShouldResetDurations> _shouldResetDurations;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Active, Duration>().Build();
            _duration = World.GetStash<Duration>();
            _parentEntity = World.GetStash<ParentEntity>();
            _shouldResetDurations = World.GetStash<ShouldResetDurations>();
            _cancelled = World.GetStash<Cancelled>();
            _doNotDeactivateWhenParentChannelEnds = World.GetStash<DoNotDeactivateWhenParentChannelEnds>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var duration = ref _duration.Get(entity);
                ref var parent = ref _parentEntity.Get(entity);

                if (!_doNotDeactivateWhenParentChannelEnds.Has(entity))
                    if (_shouldResetDurations.Has(parent.Entity) || _cancelled.Has(parent.Entity)) {
                        duration.Elapsed = 0;
                        duration.PercentElapsed = 0;

                                                    // Debug.Log("Duration reset");
                        continue;
                    }

                duration.Elapsed += deltaTime;
                duration.PercentElapsed = duration.Elapsed / duration.Max;
            }
        }
    }
}