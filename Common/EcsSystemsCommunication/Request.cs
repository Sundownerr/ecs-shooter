using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Systems
{
    public static class Request
    {
        private static World _world;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearOnReload() =>
            _world = null;

        public static void Initialize(World world) =>
            _world = world;

        public static Entity CreateRequest<T>(this Stash<T> stash, in T component) where T : struct, IComponent
        {
            var entity = _world.CreateEntity();
             stash.Set(entity, component);
            return entity;
        }

        public static Entity CreateRequest<T>(this Stash<T> stash) where T : struct, IComponent
        {
            var entity = _world.CreateEntity();
            stash.Add(entity);
            return entity;
        }

        public static void CompleteRequest(this Entity entity) =>
            _world.RemoveEntity(entity);
    }
}