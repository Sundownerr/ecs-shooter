using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveEnterActionResetDurationsTagSystem : ISystem
    {
        private Filter _filter;
        private Stash<ShouldResetDurations> _shouldResetDurations;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<EnterAction, ShouldResetDurations>();
            _shouldResetDurations = World.GetStash<ShouldResetDurations>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                _shouldResetDurations.Remove(entity);
        }
    }
}
