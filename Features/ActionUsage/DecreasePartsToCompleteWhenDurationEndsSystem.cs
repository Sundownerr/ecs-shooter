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
    public sealed class DecreasePartsToCompleteWhenDurationEndsSystem : ISystem
    {
        private Filter _filter;
        private Stash<Duration> _duration;
        private Stash<ParentEntity> _parentEntity;
        private Stash<PartsToComplete> _partsToComplete;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Active, UsageProgressPart, Duration>();
            _duration = World.GetStash<Duration>();
            _parentEntity = World.GetStash<ParentEntity>();
            _partsToComplete = World.GetStash<PartsToComplete>();
        }

        public World World { get; set; }

        // updates whole timeline entity, not actions
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var duration = ref _duration.Get(entity);

                if (duration.Elapsed < duration.Max)
                    continue;

                ref var parent = ref _parentEntity.Get(entity);

                if (_partsToComplete.Has(parent.Entity))
                {
                    ref var partsToComplete = ref _partsToComplete.Get(parent.Entity);
                    partsToComplete.Value--;
                }

                // Debug.Log( $"Channel completed");
            }
        }
    }
}
