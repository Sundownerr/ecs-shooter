using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CheckEnemySpawnRequestSystem : ISystem
    {
        private Stash<EnemiesAlive> _enemiesAlive;
        private Filter _enemiesAliveFilter;
        private Filter _filter;
        private Stash<Request_CreateNpc> _requestCreateNpc;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_CreateNpc>();
            _enemiesAlive = World.GetStash<EnemiesAlive>();
            _requestCreateNpc = World.GetStash<Request_CreateNpc>();
            _enemiesAliveFilter = World.Filter.With<EnemiesAlive>().Build();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var request = ref _requestCreateNpc.Get(entity);
                ref var enemiesAlive = ref _enemiesAlive.Get(_enemiesAliveFilter.First());

                if (enemiesAlive.All[request.Config.ID].Count >= request.Config.MaxAmount)
                    entity.CompleteRequest();
            }
        }
    }
}