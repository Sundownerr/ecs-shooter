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
    public sealed class LerpMovementOvershootSystem : ISystem
    {
        private Stash<Active> _active;
        private Stash<Duration> _duration;
        private Filter _filter;
        private Stash<LerpMovementOvershootTimer> _lerpMovementOvershootTimer;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<MoveTransform, Active, Duration, LerpMovementOvershootTimer>();

            _active = World.GetStash<Active>();
            _lerpMovementOvershootTimer = World.GetStash<LerpMovementOvershootTimer>();
            _duration = World.GetStash<Duration>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var duration = ref _duration.Get(entity);

                if (duration.PercentElapsed <= 1)
                    continue;

                ref var timer = ref _lerpMovementOvershootTimer.Get(entity);
                timer.Elapsed += deltaTime;

                if (timer.Elapsed >= timer.Max)
                    _active.Remove(entity);
            }
        }
    }
}