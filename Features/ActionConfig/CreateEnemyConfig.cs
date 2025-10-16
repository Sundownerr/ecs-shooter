using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct CreateEnemyConfig
    {
        [HideLabel] public NpcConfig Enemy;
        [HideLabel] public PositionConfig Position;
        [HideLabel] public RotationConfig Rotation;

        public void AddTo(Entity entity)
        {
            ref var createEnemy = ref entity.AddComponent<AbilityCreateEnemy>();
            createEnemy.Config = this;

            Position.AddTo(entity);
            Rotation.AddTo(entity);
        }
    }
}