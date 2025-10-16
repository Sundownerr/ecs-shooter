using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Init_MoveTransformInitialPosition : ISystem
    {
        private Filter _filter;
        private Stash<InitialPosition> _initialPosition;
        private Stash<TransformFromConfig> _transformFromConfig;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveTransform, Active, InitialPosition>()
                .Without<Initialized>().Build();
            _initialPosition = World.GetStash<InitialPosition>();
            _transformFromConfig = World.GetStash<TransformFromConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var initialPosition = ref _initialPosition.Get(entity);
                ref var transformFromConfig = ref _transformFromConfig.Get(entity);
                initialPosition.Value = transformFromConfig.Transform.position;
            }
        }
    }
}