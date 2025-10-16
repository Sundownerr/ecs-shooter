using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyPlayerSystem : ISystem
    {
        private Filter _filter;
        private Stash<Player> _player;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, WillBeDestroyed>();
            _player = World.GetStash<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var player = ref _player.Get(entity);

                Object.Destroy(player.Instance.gameObject);
                World.RemoveEntity(player.Instance.Entity);
            }
        }
    }
}