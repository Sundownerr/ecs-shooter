using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyUsageProgressSystem : ISystem
    {
        private Filter _filter;
        private Stash<UsageProgress> _usageProgress;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<UsageProgress, WillBeDestroyed>();
            _usageProgress = World.GetStash<UsageProgress>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var usageProgress = ref _usageProgress.Get(entity);
                World.RemoveEntity(usageProgress.Entity);
            }
        }
    }
}
