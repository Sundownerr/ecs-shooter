using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveDeadEnemyFromListSystem : ISystem
    {
        private Filter _currentLevelFilter;

        private Stash<EnemiesAlive> _enemiesAlive;
        private Filter _filter;

        private Stash<Npc> _npc;

        public void Dispose() { }

        public void OnAwake()
        {
            _npc = World.GetStash<Npc>();
            _enemiesAlive = World.GetStash<EnemiesAlive>();

            _filter = Entities.With<Npc, WillBeDestroyed>();
            _currentLevelFilter = Entities.With<CurrentLevel>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var enemy = ref _npc.Get(entity);
                var currentLevelEntity = _currentLevelFilter.First();
                ref var enemiesAlive = ref _enemiesAlive.Get(currentLevelEntity);

                enemiesAlive.All[enemy.Config.ID].Remove(enemy.Instance);
            }
        }
    }
}