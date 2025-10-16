using System;
using EcsMagic.Actions;
using Game;
using Game.Components;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct GameObjectActive_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public GameObjectConfig GameObjectConfig;

        [HideLabel]
        [HorizontalGroup("GameObjectActiveConfig")]
        public bool CheckForActive; // true = check if active, false = check if inactive

        public void AddTo(Entity entity)
        {
            StaticStash.GameObjectActive.Set(entity, new GameObjectActive {CheckForActive = CheckForActive,});

            StaticStash.GameObjectFromConfig.Set(entity, new GameObjectFromConfig {Config = GameObjectConfig,});
        }
    }
}