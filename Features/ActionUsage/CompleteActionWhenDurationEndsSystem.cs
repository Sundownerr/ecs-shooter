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
    public sealed class CompleteActionWhenDurationEndsSystem : ISystem
    {
        private Filter _filter;
        private Stash<Duration> _duration;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Active, Duration>().Without<DoNotDeactivateWhenDurationEnds>().Build();
            _duration = World.GetStash<Duration>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var duration = ref _duration.Get(entity);

                if (duration.Elapsed < duration.Max)
                    continue;

                duration.Elapsed = 0;
                duration.PercentElapsed = 0;

                _active.Remove(entity);
            }
        }
    }
}
