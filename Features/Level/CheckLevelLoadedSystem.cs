using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CheckLevelLoadedSystem : ISystem
    {
        private Stash<Event_CompletedLoadingScene> _eventCompletedLoadingScene;
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<LoadingSceneHandle>();
            _eventCompletedLoadingScene = World.GetStash<Event_CompletedLoadingScene>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var loadingSceneHandle = ref entity.GetComponent<LoadingSceneHandle>();

                if (!loadingSceneHandle.Handle.IsDone)
                    continue;

                _eventCompletedLoadingScene.CreateEvent(new Event_CompletedLoadingScene
                    {Handle = loadingSceneHandle.Handle,});
                
                World.RemoveEntity(entity);
                
            }
        }
    }
}