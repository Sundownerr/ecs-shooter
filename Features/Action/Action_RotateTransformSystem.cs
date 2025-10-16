using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_RotateTransformSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<RotationFromConfig> _rotationFromConfig;
        private Stash<TransformFromConfig> _transformFromConfig;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<RotateTransform, Active>();

            // Initialize stashes
            _rotationFromConfig = World.GetStash<RotationFromConfig>();
            _transformFromConfig = World.GetStash<TransformFromConfig>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rotationFromConfig = ref _rotationFromConfig.Get(entity);
                ref var transformFromConfig = ref _transformFromConfig.Get(entity);

                transformFromConfig.Transform.rotation = rotationFromConfig.Rotation;

                // _active.Remove(entity);
            }
        }
    }
}
