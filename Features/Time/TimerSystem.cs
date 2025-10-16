using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TimerSystem : ISystem
    {
        private Filter _filter;
        private Stash<IncreasingTimer> _increasingTimer;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<IncreasingTimer>()
                .Without<TimerCompleted>()
                .Build();

            _increasingTimer = World.GetStash<IncreasingTimer>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var timer = ref _increasingTimer.Get(entity);
                timer.Elapsed += deltaTime;
            }
        }
    }
}
