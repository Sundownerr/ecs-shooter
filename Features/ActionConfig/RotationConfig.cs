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
    public struct RotationConfig
    {
        [LabelText("Rotation")] [LabelWidth(50)]
        public Rotation Value;
        [HideLabel] [ShowIf(nameof(IsCustomTransform))]
        public Transform CustomTransform;

        private bool IsCustomTransform() => Value == Rotation.CustomTransform;

        public void AddTo(Entity entity)
        {
            entity.SetComponent(new RotationFromConfig {Config = this,});

            if (Value is not Rotation.CustomRotation)
                entity.AddComponent<LastRotation>();

            if (Value is Rotation.TargetTransform or Rotation.TowardsTarget)
                if (!entity.Has<TargetEntity>())
                    entity.AddComponent<TargetEntity>();
        }
    }
}