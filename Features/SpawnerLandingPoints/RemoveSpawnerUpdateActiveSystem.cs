using EcsMagic.CommonComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveSpawnerUpdateActiveSystem : ISystem
    {
        private Stash<Active> _active;
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<SpawnerLandingPointsUpdateEntity, Active>();

            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                _active.Remove(entity);
        }
    }
}