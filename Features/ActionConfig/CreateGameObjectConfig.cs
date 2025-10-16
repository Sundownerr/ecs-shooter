using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct CreateGameObjectConfig
    {
        [OnValueChanged(nameof(CheckPrefab))]
        [HideLabel] public GameObject Prefab;
        [HideLabel] [InlineProperty] public PositionConfig Position;
        [HideLabel] [InlineProperty] public RotationConfig Rotation;

        [ShowIf(nameof(PassTargets))]
        public bool PassUserTargets;
        [ShowIf(nameof(PassTargets))]
        public bool PassTargetsOnlyOnce;

        // New field for pooling
        public bool UsePooling;

        [HideInInspector] public bool IsAbilityProvider;
        [HideInInspector] public bool IsStateMachineProvider;

        private bool PassTargets() => IsAbilityProvider || IsStateMachineProvider;

        private void CheckPrefab()
        {
            IsAbilityProvider = Prefab.GetComponent<AbilityProvider>() != null;
            IsStateMachineProvider = Prefab.GetComponent<StateMachineProvider>() != null;
        }

        public void AddTo(Entity entity)
        {
            Position.AddTo(entity);
            Rotation.AddTo(entity);

            if (!PassTargets())
            {
                entity.SetComponent(new CreateGameObject
                {
                    Prefab = Prefab,
                    UsePooling = UsePooling,
                });
            }
            else
            {
                if (IsAbilityProvider)
                    entity.SetComponent(new CreateAbilityProvider
                    {
                        Prefab = Prefab.GetComponent<AbilityProvider>(),
                        PassUserTargets = PassUserTargets,
                        PassTargetsOnlyOnce = PassTargetsOnlyOnce,
                        UsePooling = UsePooling,
                    });

                if (IsStateMachineProvider)
                    entity.SetComponent(new CreateStateMachineProvider
                    {
                        Prefab = Prefab.GetComponent<StateMachineProvider>(),
                        PassUserTargets = PassUserTargets,
                        PassTargetsOnlyOnce = PassTargetsOnlyOnce,
                        UsePooling = UsePooling,
                    });
            }
        }
    }
}