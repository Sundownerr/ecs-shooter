using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkNpcWillBeDestroyedSystem : ISystem
    {
        private Filter _filter;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Npc>().With<Dead>().Without<WillBeDestroyed>().Build();
            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                _willBeDestroyed.Add(entity);
        }
    }
}
