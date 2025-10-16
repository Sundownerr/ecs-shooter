using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateNpcPositionSystem : ISystem
    {
        private Filter _filter;
        private Stash<Reference<Transform>> _transformStash;
        private Stash<WorldPosition> _worldPositionStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Npc, WorldPosition, Reference<Transform>>();
            
            _worldPositionStash = World.GetStash<WorldPosition>();
            _transformStash = World.GetStash<Reference<Transform>>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var worldPosition = ref _worldPositionStash.Get(entity);
                ref Reference<Transform> transform = ref _transformStash.Get(entity);

                worldPosition.Value = transform.Value.position;
            }
        }
    }
}