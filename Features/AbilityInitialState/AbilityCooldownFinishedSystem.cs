using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilityCooldownFinishedSystem : ISystem
    {
        private Stash<CooldownFinished> _cooldownFinished;
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<AbilityCooldown, TimerCompleted>().Without<CooldownFinished>()
                .Build();
            _cooldownFinished = World.GetStash<CooldownFinished>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                _cooldownFinished.Add(entity);
        }
    }
}