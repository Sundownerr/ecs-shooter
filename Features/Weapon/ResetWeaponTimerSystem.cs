using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetWeaponTimerSystem : ISystem
    {
        private Filter _filter;
        private Stash<TimerCompleted> _timerCompleted;
        private Stash<IncreasingTimer> _increasingTimer;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Weapon, TimerCompleted, WeaponTriggerPulled>();
            _timerCompleted = World.GetStash<TimerCompleted>();
            _increasingTimer = World.GetStash<IncreasingTimer>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var timer = ref _increasingTimer.Get(entity);
                timer.Elapsed = 0f;

                _timerCompleted.Remove(entity);
            }
        }
    }
}
