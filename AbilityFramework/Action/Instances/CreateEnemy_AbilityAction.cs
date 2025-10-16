using System;
using Ability;
using Game;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct CreateEnemy_AbilityAction : IAbilityAction
    {
        [HideLabel] public NpcConfig Enemy;
        [HideLabel] public PositionConfig Position;
        [HideLabel] public RotationConfig Rotation;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityCreateEnemy.Set(entity, new AbilityCreateEnemy
            {
                Config = new CreateEnemyConfig
                {
                    Enemy = Enemy,
                    Position = Position,
                    Rotation = Rotation
                }
            });

            Position.AddTo(entity);
            Rotation.AddTo(entity);
        }
    }
}
