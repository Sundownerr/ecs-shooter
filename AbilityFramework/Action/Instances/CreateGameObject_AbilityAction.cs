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
    public struct CreateGameObject_AbilityAction : IAbilityAction
    {
        [HideInInspector] public bool IsAbilityProvider;
        [HideInInspector] public bool IsStateMachineProvider;

        [OnValueChanged(nameof(CheckPrefab), true)]
        [HideLabel]
        public GameObject Prefab;

        [HideLabel] [InlineProperty]
        public PositionConfig Position;

        [HideLabel] [InlineProperty]
        public RotationConfig Rotation;

        public bool UsePooling;

        [ShowIf(nameof(PassTargets))]
        public bool PassUserTargets;

        [ShowIf(nameof(PassTargets))]
        public bool PassTargetsOnlyOnce;
        
        public bool DontDestroyWhenWorldChanges;

        public void AddTo(Entity entity)
        {
            Position.AddTo(entity);
            Rotation.AddTo(entity);

            if (!PassTargets()) {
                StaticStash.CreateGameObject.Set(entity, new CreateGameObject {
                    Prefab = Prefab,
                    UsePooling = UsePooling,
                });
            }
            else {
                if (IsAbilityProvider)
                    StaticStash.CreateAbilityProvider.Set(entity, new CreateAbilityProvider {
                        Prefab = Prefab.GetComponent<AbilityProvider>(),
                        PassUserTargets = PassUserTargets,
                        PassTargetsOnlyOnce = PassTargetsOnlyOnce,
                        UsePooling = UsePooling,
                        DontDestroyWhenWorldChanges = DontDestroyWhenWorldChanges,
                    });

                if (IsStateMachineProvider)
                    StaticStash.CreateStateMachineProvider.Set(entity, new CreateStateMachineProvider {
                        Prefab = Prefab.GetComponent<StateMachineProvider>(),
                        PassUserTargets = PassUserTargets,
                        PassTargetsOnlyOnce = PassTargetsOnlyOnce,
                        UsePooling = UsePooling,
                        DontDestroyWhenWorldChanges = DontDestroyWhenWorldChanges,
                    });
            }
        }

        private bool PassTargets() => IsAbilityProvider || IsStateMachineProvider;

        public void CheckPrefab()
        {
            if (Prefab == null)
                return;

            IsAbilityProvider = Prefab.GetComponent<AbilityProvider>() != null;
            IsStateMachineProvider = Prefab.GetComponent<StateMachineProvider>() != null;
        }
    }

    [Flags]
    public enum CreateOptions
    {
        None = 0,
        UsePooling = 1 << 0,
        PassUserTargetsAlways = 1 << 1,
        PassUserTargetsOnce = 1 << 2,
        DontDestroyWhenWorldChanges = 1 << 3
    }
}