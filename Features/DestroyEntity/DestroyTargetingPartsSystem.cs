using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyTargetingPartsSystem : ISystem
    {
        private Filter _filter;
        private Stash<TargetingParts> _targetingParts;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<TargetingParts, WillBeDestroyed>();
            _targetingParts = World.GetStash<TargetingParts>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parts = ref _targetingParts.Get(entity);

                foreach (var part in parts.List)
                    World.RemoveEntity(part);
            }
        }
    }
}
