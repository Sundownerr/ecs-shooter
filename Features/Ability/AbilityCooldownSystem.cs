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
    public sealed class AbilityCooldownSystem : ISystem
    {
        private Filter _filter;
        private Stash<Active> _active;
        private Stash<IncreasingTimer> _increasingTimer;
        private Stash<TimerCompleted> _timerCompleted;
        private Stash<CooldownFinished> _cooldownFinished;
        private Stash<ParentEntity> _parentEntity;
        private Stash<PartsToComplete> _partsToComplete;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<AbilityCooldown>().With<Active>().Build();
            
            _active = World.GetStash<Active>();
            _increasingTimer = World.GetStash<IncreasingTimer>();
            _timerCompleted = World.GetStash<TimerCompleted>();
            _cooldownFinished = World.GetStash<CooldownFinished>();
            _parentEntity = World.GetStash<ParentEntity>();
            _partsToComplete = World.GetStash<PartsToComplete>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var timer = ref _increasingTimer.Get(entity);
                timer.Elapsed = 0;
                _timerCompleted.Remove(entity);

                if (_cooldownFinished.Has(entity))
                    _cooldownFinished.Remove(entity);

                ref var parent = ref _parentEntity.Get(entity);

                ref var partsToComplete = ref _partsToComplete.Get(parent.Entity);
                partsToComplete.Value--;

                _active.Remove(entity);
            }
        }
    }
}
