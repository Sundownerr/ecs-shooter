using System.Collections.Generic;
using Ability;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;

namespace Game
{
    public static class AbilityProviderFactory
    {
        public static Entity Create(this AbilityProvider abilityProvider, Entity user, World world)
        {
            var abilityEntity = abilityProvider.Initialize(world);
            abilityProvider.Created = true;

            StaticStash.AbilityTag.Add(abilityEntity);
#if UNITY_EDITOR
            if (abilityProvider.Debug)
                StaticStash.Debug_AbilityLog.Set(abilityEntity,
                    new Debug_AbilityLog {AbilityProvider = abilityProvider,});
#endif

            StaticStash.ParentEntity.Set(abilityEntity, new ParentEntity {Entity = user,});
            StaticStash.Targets.Set(abilityEntity, new Targets {List = new List<Entity>(),});

            StaticStash.AbilityStateInitial.Add(abilityEntity);
            StaticStash.AbilityCustomData.Add(abilityEntity);
            StaticStash.PartsToComplete.Add(abilityEntity);
            StaticStash.AbilityActivatedFromScript.Add(abilityEntity);

            var oneShot = abilityProvider.OneShot();
            if (oneShot)
                StaticStash.OneShotAbility.Add(abilityEntity);

            if (abilityProvider.HaveCooldown()) {
                var cooldownParts = new List<Entity>(abilityProvider.NewCooldown.Count);

                foreach (var cooldownConfig in abilityProvider.NewCooldown)
                    cooldownParts.Add(cooldownConfig.CreateEntity(world, abilityEntity));

                StaticStash.CooldownParts.Set(abilityEntity, new CooldownParts {List = cooldownParts,});
            }

            if (abilityProvider.HaveTargeting()) {
                var count = abilityProvider.NewTarget.Count;
                var targetingParts = new List<Entity>(count);

                if (count > 0)
                    foreach (var targetingConfig in abilityProvider.NewTarget)
                        targetingParts.Add(targetingConfig.CreateEntity(world, abilityEntity, user, abilityEntity));

                StaticStash.TargetingParts.Set(abilityEntity, new TargetingParts {List = targetingParts,});
            }

            if (abilityProvider.HaveUseConditions() || oneShot) {
                var conditionsCount = abilityProvider.NewUseConditions.Count;

                if (oneShot)
                    conditionsCount += 1;

                StaticStash.ForwardConditions.Set(abilityEntity,
                    new ForwardConditions {List = new Entity[conditionsCount],});

                StaticStash.ForwardConditionsToMeet.Set(abilityEntity,
                    new ForwardConditionsToMeet {Total = conditionsCount,});

                for(var i = 0; i < abilityProvider.NewUseConditions.Count; i++) {
                    abilityProvider.NewUseConditions[i].CreateEntity(world, AbilityConditionFor.Forward,
                        abilityEntity, user, abilityEntity, i);
                }

                if (oneShot)
                    AbilityConditionWrapper.OneShotCondition().CreateEntity(world, AbilityConditionFor.Forward, abilityEntity,
                        user, abilityEntity, conditionsCount - 1);
            }

            abilityProvider.UsageConfig.AddTo(abilityEntity, user, abilityEntity, world);

            if (abilityProvider.IsChannelUsage()) {
                if (abilityProvider.OnUsageCancelled.Count > 0) {
                    var cancelActions = new List<Entity>(abilityProvider.OnUsageCancelled.Count);

                    foreach (var useAction in abilityProvider.OnUsageCancelled)
                        cancelActions.Add(useAction.CreateEntity(world, abilityEntity, user, abilityEntity));

                    StaticStash.CancelActions.Set(abilityEntity, new CancelActions {List = cancelActions,});
                }

                List<AbilityConditionWrapper> cancelConditions = abilityProvider.NewCancelWhen;

                if (cancelConditions.Count > 0) {
                    StaticStash.CancelConditionsToMeet.Add(abilityEntity);
                    StaticStash.CancelConditions.Set(abilityEntity,
                        new CancelConditions {List = new Entity[cancelConditions.Count],});

                    for(var i = 0; i < cancelConditions.Count; i++) {
                        cancelConditions[i].CreateEntity(world, AbilityConditionFor.CancelingAbility, abilityEntity,
                            user,
                            abilityEntity, i);
                    }
                }
            }

            if (!abilityProvider.ActiveOnStart)
                abilityProvider.Deactivate();

            return abilityEntity;
        }
    }
}