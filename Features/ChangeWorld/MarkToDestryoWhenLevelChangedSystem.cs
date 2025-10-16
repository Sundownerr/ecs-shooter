using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkToDestryoWhenLevelChangedSystem : ISystem
    {
        private Filter _eventFilter;
        private Filter _filter;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _eventFilter = Entities.With<Event_LevelChanged>();
            
            _filter = Entities.With<MarkToDestroyWhenLevelChanged>();
            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var _ in _eventFilter) {
                foreach (var entity in _filter) {
                    if (!_willBeDestroyed.Has(entity))
                        _willBeDestroyed.Add(entity);
                }
            }
        }
    }
}