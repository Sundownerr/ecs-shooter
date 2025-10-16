using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerPositionSystem : ISystem
    {
        private Filter _filter;
        private Stash<LastNavMeshPosition> _lastNavMeshPosition;
        private Stash<Reference<Transform>> _referenceTransform;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, WorldPosition, LastNavMeshPosition>();
            _worldPosition = World.GetStash<WorldPosition>();
            _referenceTransform = World.GetStash<Reference<Transform>>();
            _lastNavMeshPosition = World.GetStash<LastNavMeshPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var worldPosition = ref _worldPosition.Get(entity);
                ref Reference<Transform> transform = ref _referenceTransform.Get(entity);
                
                worldPosition.Value = transform.Value.position;

                ref var lastNavMeshPosition = ref _lastNavMeshPosition.Get(entity);

                if (NavMesh.SamplePosition(worldPosition.Value, out var hit, 1f, NavMesh.AllAreas)) {
                    lastNavMeshPosition.Value = hit.position;
                }
                else {
                    var hitFloor = Physics.Raycast(worldPosition.Value, Vector3.down, out var hitInfo, 200f, 1 << 6);

                    if (hitFloor && NavMesh.SamplePosition(hitInfo.point, out var rayHit, 1f, NavMesh.AllAreas))
                        lastNavMeshPosition.Value = rayHit.position;
                }
            }
        }
    }
}