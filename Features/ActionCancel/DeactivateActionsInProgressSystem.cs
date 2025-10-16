using System.Collections.Generic;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DeactivateActionsInProgressSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<Cancelled, UsageProgress, UsageParts>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var usageProgress = ref entity.GetComponent<UsageProgress>();
                usageProgress.Entity.RemoveComponent<Active>();

                ref var usageParts = ref entity.GetComponent<UsageParts>();

                foreach ((var config, List<Entity> entities) in usageParts.UsagePartActions) {
                    foreach (var action in entities)
                        action.RemoveComponent<Active>();
                }
            }
        }
    }
}