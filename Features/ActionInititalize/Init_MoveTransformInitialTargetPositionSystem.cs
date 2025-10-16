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
    public sealed class Init_MoveTransformInitialTargetPositionSystem : ISystem
    {
        private Filter _filter;
        private Stash<InitialTargetPosition> _initialTargetPosition;
        private Stash<PositionFromConfig> _positionFromConfig;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveTransform, Active, InitialTargetPosition>()
                .Without<Initialized>().Build();
            _initialTargetPosition = World.GetStash<InitialTargetPosition>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var initialPosition = ref _initialTargetPosition.Get(entity);
                ref var config = ref _positionFromConfig.Get(entity);
                initialPosition.Value = config.Position;
            }
        }
    }
}
