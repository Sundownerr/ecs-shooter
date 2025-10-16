using System;
using System.Collections.Generic;
using EcsMagic.Abilities;
using EcsMagic.Actions;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public static class ActionExtentions
    {
        public static void AddRigidbodyTargets(this RigidbodyConfig config,
                                               Entity entity,
                                               List<Rigidbody> _targets)
        {
            _targets.Clear();

            switch (config.TargetType) {
                case TargetType.Self:
                    ref var user = ref entity.GetComponent<UserEntity>();
                    ref Reference<Rigidbody> userRigidbody = ref user.Entity.GetComponent<Reference<Rigidbody>>();
                    _targets.Add(userRigidbody.Value);
                    break;

                case TargetType.Other:
                    _targets.Add(config.OtherTarget);
                    break;

                case TargetType.Target:
                    ref var targetsProvdier = ref entity.GetComponent<TargetsProviderEntity>();
                    ref var targets = ref targetsProvdier.Entity.GetComponent<Targets>();

                    foreach (var target in targets.List) {
                        if (target.IsNullOrDisposed() || !target.Has<Reference<Rigidbody>>())
                            continue;

                        ref Reference<Rigidbody> targetRigidbody = ref target.GetComponent<Reference<Rigidbody>>();
                        _targets.Add(targetRigidbody.Value);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool WorldSpace(this Direction direction) =>
            direction switch {
                Direction.TargetAwayFromUser => false,
                Direction.TargetTowardsUser => false,
                Direction.CustomWorldDirection => true,
                Direction.CustomLocalDirection => false,
                _ => throw new ArgumentOutOfRangeException(),
            };
    }
}