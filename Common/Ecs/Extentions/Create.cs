using Scellecs.Morpeh;

namespace Game
{
    public static class Create
    {
        private static World _world;

        public static ref T Singleton<T>() where T : struct, IComponent
        {
            var entity = _world.CreateEntity();
            return ref entity.AddComponent<T>(); 
        }

        public static void Initialize(World world) => _world = world;
    }
}