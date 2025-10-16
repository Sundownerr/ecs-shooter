using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CleanupEcsSystem : ISystem
    {
        public void Dispose() { }

        public void OnAwake() { }

        public World World { get; set; }

        public void OnUpdate(float deltaTime) {}
    }
}