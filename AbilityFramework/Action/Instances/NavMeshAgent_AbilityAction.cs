using System;
using EcsMagic.Actions;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public struct NavMeshAgent_AbilityAction : IAbilityAction
    {
        

        [HorizontalGroup("0")]
        [HideLabel]
        public NavMeshAgentConfig.NavMeshAgentAction Action;

        [HorizontalGroup("0")]
        [HideLabel]
        [ShowIf(nameof(IsChangeSpeed))]
        public float Speed;

        private bool IsChangeSpeed() => Action == NavMeshAgentConfig.NavMeshAgentAction.SetSpeed;

        public void AddTo(Entity entity)
        {
            switch (Action)
            {
                case NavMeshAgentConfig.NavMeshAgentAction.Enable:
                    StaticStash.EnableUserNavMeshAgent.Add(entity);
                    break;
                case NavMeshAgentConfig.NavMeshAgentAction.Disable:
                    StaticStash.DisableUserNavMeshAgent.Add(entity);
                    break;
                case NavMeshAgentConfig.NavMeshAgentAction.SetSpeed:
                    StaticStash.ChangeUserNavMeshAgentSpeed.Set(entity, new ChangeUserNavMeshAgentSpeed
                    {
                        Speed = Speed
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
