using System.Collections.Generic;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyUsagePartsSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<UsageParts, WillBeDestroyed>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var usageParts = ref entity.GetComponent<UsageParts>();

                foreach ((var config, List<Entity> parts) in usageParts.UsagePartActions)
                foreach (var part in parts)
                    World.RemoveEntity(part);
            }
        }
    }
}