using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ActivateStateChannelActionsSystem : ISystem
    {
        private Filter _filter;
        private Stash<UsageProgress> _usageProgress;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NeedsActivation, UsageProgress>();
            _usageProgress = World.GetStash<UsageProgress>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var usageType = ref _usageProgress.Get(entity);
                _active.Add(usageType.Entity);
            }
        }
    }
}
