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
    public sealed class DestroyNpcSystem : ISystem
    {
        private Filter _filter;
        private Stash<Npc> _npc;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Npc, WillBeDestroyed>();
            _npc = World.GetStash<Npc>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var enemy = ref _npc.Get(entity);

                Object.Destroy(enemy.Instance.gameObject);
                World.RemoveEntity(enemy.Instance.Entity);
            }
        }
    }
}
