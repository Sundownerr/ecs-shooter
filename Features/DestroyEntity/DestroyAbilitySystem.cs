using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyAbilitySystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<AbilitiesList, WillBeDestroyed>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var abilities = ref entity.GetComponent<AbilitiesList>();

                foreach (var ability in abilities.List)
                    World.RemoveEntity(ability);
            }
        }
    }
}