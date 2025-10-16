using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveInitializedTagOnDeactivatedPartsSystem : ISystem
    {
        private Filter _filter;
        private Stash<Initialized> _initialized;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Initialized>().Without<Active>().Build();
            _initialized = World.GetStash<Initialized>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                _initialized.Remove(entity);
        }
    }
}
