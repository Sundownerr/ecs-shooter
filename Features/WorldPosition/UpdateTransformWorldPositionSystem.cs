using EcsMagic.CommonComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateTransformWorldPositionSystem : ISystem
    {
        private Filter _filter;
        private Stash<Reference<Transform>> _transform;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WorldPosition, Reference<Transform>>();
            _worldPosition = World.GetStash<WorldPosition>();
            _transform = World.GetStash<Reference<Transform>>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var worldPosition = ref _worldPosition.Get(entity);
                ref Reference<Transform> transform = ref _transform.Get(entity);

                worldPosition.Value = transform.Value.position;
            }
        }
    }
}