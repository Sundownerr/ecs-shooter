using System;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct NavMeshAgentConfig
    {
        public enum NavMeshAgentAction
        {
            Enable = 0, Disable = 1, SetSpeed = 2,
        }

        [HorizontalGroup("0")] [HideLabel]
        public NavMeshAgentAction Action;

        [HorizontalGroup("0")] [HideLabel]
        [ShowIf(nameof(IsChangeSpeed))]
        public float Speed;

        private bool IsChangeSpeed() => Action == NavMeshAgentAction.SetSpeed;

        public void AddTo(Entity entity)
        {
            switch (Action) {
                case NavMeshAgentAction.Enable:
                    entity.AddComponent<EnableUserNavMeshAgent>();
                    break;
                case NavMeshAgentAction.Disable:
                    entity.AddComponent<DisableUserNavMeshAgent>();
                    break;
                case NavMeshAgentAction.SetSpeed:
                    ref var changeSpeed = ref entity.AddComponent<ChangeUserNavMeshAgentSpeed>();
                    changeSpeed.Speed = Speed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}