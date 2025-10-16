using System;
using EcsMagic.Abilities;
using EcsMagic.Actions;
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
    public sealed class Action_ChangeAnimatorSettingsSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<UserEntity> _userEntity;
        private Stash<ChangeAnimatorSettings> _changeAnimatorSettings;
        private Stash<Reference<Animator>> _animatorReference;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ChangeAnimatorSettings, Active>();

            // Initialize stashes
            _userEntity = World.GetStash<UserEntity>();
            _changeAnimatorSettings = World.GetStash<ChangeAnimatorSettings>();
            _animatorReference = World.GetStash<Reference<Animator>>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);
                ref var changeAnimatorSettings = ref _changeAnimatorSettings.Get(entity);

                var config = changeAnimatorSettings.Config;
                Animator targetAnimator;

                if (config._targetType == TargetType.Self)
                {
                    ref Reference<Animator> userAnimator = ref _animatorReference.Get(user.Entity);
                    targetAnimator = userAnimator.Value;
                }
                else
                {
                    targetAnimator = config.Other;
                }

                var parameterName = config.ParameterName;

                switch (config.Type)
                {
                    case ChangeAnimatorSettignsConfig.ChangeType.SetBool:
                        targetAnimator.SetBool(parameterName, config.BoolValue);
                        break;

                    case ChangeAnimatorSettignsConfig.ChangeType.SetFloat:
                        targetAnimator.SetFloat(parameterName, config.FloatValue);
                        break;

                    case ChangeAnimatorSettignsConfig.ChangeType.SetInt:
                        targetAnimator.SetInteger(parameterName, config.IntValue);
                        break;

                    case ChangeAnimatorSettignsConfig.ChangeType.SetTrigger:
                        targetAnimator.SetTrigger(parameterName);
                        break;

                    case ChangeAnimatorSettignsConfig.ChangeType.SetAnimatorSpeed:
                        targetAnimator.speed = config.AnimatorSpeedValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _active.Remove(entity);
            }
        }
    }
}
