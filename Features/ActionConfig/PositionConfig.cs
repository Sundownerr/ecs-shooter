using System;
using EcsMagic.CommonComponents;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct PositionConfig
    {
        [LabelText("At")] [LabelWidth(20)] public Position Value;
        [HideLabel] [ShowIf(nameof(IsCustomTransform))]
        public Transform CustomTransform;

        private bool IsCustomTransform() => Value == Position.CustomTransform;

        public void AddTo(Entity entity)
        {
            ref var positionFromConfig = ref entity.AddComponent<PositionFromConfig>();
            positionFromConfig.Config = this;
            
            if (Value is Position.TargetTransform)
                if (!entity.Has<TargetEntity>())
                    entity.AddComponent<TargetEntity>();
        }
    }
}