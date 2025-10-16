using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyEntities<T> : ISystem 
        where T : struct, IComponent
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<T>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                World.RemoveEntity(entity);
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyEntities<T1, T2> : ISystem 
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<T1, T2>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                World.RemoveEntity(entity);
        }
    }
}