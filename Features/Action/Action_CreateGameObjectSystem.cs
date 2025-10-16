using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_CreateGameObjectSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<CreateGameObject> _createGameObject;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<RotationFromConfig> _rotationFromConfig;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CreateGameObject, Active>();

            // Initialize stashes
            _createGameObject = World.GetStash<CreateGameObject>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _rotationFromConfig = World.GetStash<RotationFromConfig>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var action = ref _createGameObject.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);
                ref var rotationFromConfig = ref _rotationFromConfig.Get(entity);
                
                if (action.UsePooling)
                    GameObjectPool.Get(action.Prefab, positionFromConfig.Position,
                        rotationFromConfig.Rotation);
                else
                    Object.Instantiate(action.Prefab, positionFromConfig.Position, rotationFromConfig.Rotation);

                _active.Remove(entity);
            }
        }
    }
}
