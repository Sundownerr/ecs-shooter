using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddEnemyToAliveEnemies : ISystem
    {
        private Filter _currentLevelFilter;
        private Filter _filter;
        private Stash<Event_NpcCreated> _eventNpcCreated;
        private Stash<EnemiesAlive> _enemiesAlive;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_NpcCreated>();
            _currentLevelFilter = Entities.With<CurrentLevel>();

            _eventNpcCreated = World.GetStash<Event_NpcCreated>();
            _enemiesAlive = World.GetStash<EnemiesAlive>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            // foreach (var entity in _filter)
            // {
            //     ref var evt = ref _eventNpcCreated.Get(entity);
            //
            //     ref var enemiesAlive = ref _enemiesAlive.Get(_currentLevelFilter.First());
            //     enemiesAlive.All[evt.Config.ID].Add(evt.NpcInstance);
            // }
        }
    }
}
