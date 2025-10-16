using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_GameObjectActiveSystem : ISystem
    {
        private Filter _filter;
        private Stash<GameObjectActive> _gameObjectActive;
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Stash<GameObjectFromConfig> _gameObjectFromConfig;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<GameObjectActive, GameObjectFromConfig, Active>();
            _gameObjectActive = World.GetStash<GameObjectActive>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
            _gameObjectFromConfig = World.GetStash<GameObjectFromConfig>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var gameObjectActive = ref _gameObjectActive.Get(entity);
                ref var gameObjectFromConfig = ref _gameObjectFromConfig.Get(entity);

                // Skip if GameObject is not set yet
                if (gameObjectFromConfig.GameObject == null)
                    continue;

                // Check if GameObject is active or inactive based on the CheckForActive flag
                bool isActive = gameObjectFromConfig.GameObject.activeSelf;
                bool conditionMet = (gameObjectActive.CheckForActive && isActive) ||
                                   (!gameObjectActive.CheckForActive && !isActive);

                if (conditionMet)
                {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}
