using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_DestroyGameObjectSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<DestroyGameObject, Active>();
        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var destroyGameObject = ref entity.GetComponent<DestroyGameObject>();

                if (destroyGameObject.Config.Target != null)
                {
                    Object.Destroy(destroyGameObject.Config.Target);
                }

                entity.RemoveComponent<Active>();
            }
        }
    }
}
