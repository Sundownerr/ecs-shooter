using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public static class ResetStateExtentions
    {
        private static readonly List<AbilityUsageEntry> AbilityUsageEntries = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearOnReload() =>
            AbilityUsageEntries.Clear();

        public static void ResetState(this AbilityProvider abilityProvider)
        {
            abilityProvider.Entity.ResetAbilityProviderState();
            abilityProvider.Deactivate();
        }

        public static void ResetState(this StateMachineProvider stateMachineProvider) =>
            stateMachineProvider.ResetStateMachineState();

        private static void ResetStateMachineState(this StateMachineProvider stateMachineProvider)
        {
            var stateMachineEntity = stateMachineProvider.Entity;

            ref var stateMachine = ref StaticStash.StateMachine.Get(stateMachineEntity);
            stateMachine.PreviousTransition = default;

            foreach (var transitionEntity in stateMachine.Transitions) {
                ResetForwardConditions(transitionEntity);

                ResetUsageParts(transitionEntity);
                ResetUsageProgress(transitionEntity);
                ResetInstantActions(transitionEntity);

                StaticStash.NeedsActivation.Remove(transitionEntity);
                StaticStash.Active.Remove(transitionEntity);
            }

            foreach (var enterActionEntity in stateMachine.EnterActions) {
                ResetUsageParts(enterActionEntity);
                ResetUsageProgress(enterActionEntity);
                ResetInstantActions(enterActionEntity);

                StaticStash.NeedsActivation.Remove(enterActionEntity);
                StaticStash.Active.Remove(enterActionEntity);
            }

            foreach (var transitionConfig in stateMachineProvider.Transitions) {
                foreach (var action in transitionConfig.Actions) {
                    foreach (var abilityProvider in action.AbilityProviders)
                        abilityProvider.ResetState();
                }
            }

            foreach (var enterActions in stateMachineProvider.EnterActions) {
                foreach (var action in enterActions.Actions) {
                    foreach (var abilityProvider in action.AbilityProviders)
                        abilityProvider.ResetState();
                }
            }

            ref var exitTime = ref StaticStash.StateMachineExitTime.Get(stateMachineEntity);
            exitTime.Remaining = 0;

            StaticStash.ChangeState.Remove(stateMachineEntity);
        }

        private static void ResetAbilityProviderState(this Entity Entity)
        {
            ref var customData = ref StaticStash.AbilityCustomData.Get(Entity);
            customData.CustomPosition = Vector3.zero;
            customData.CustomRotation = Quaternion.identity;
            customData.CustomDirection = Vector3.zero;

            ref var targets = ref StaticStash.Targets.Get(Entity);
            targets.List.Clear();

            StaticStash.AbilityState_CheckUseConditions.Remove(Entity);
            StaticStash.AbilityState_StartCooldown.Remove(Entity);
            StaticStash.AbilityState_Activating.Remove(Entity);
            StaticStash.AbilityState_Using.Remove(Entity);

            if (!StaticStash.AbilityState_Initial.Has(Entity))
                StaticStash.AbilityState_Initial.Add(Entity);

            StaticStash.Cancelled.Remove(Entity);

            ResetUsageParts(Entity);
            ResetUsageProgress(Entity);
            ResetForwardConditions(Entity);
            ResetCancelConditions(Entity);
            ResetInstantActions(Entity);
        }

        private static void ResetInstantActions(Entity entity)
        {
            if (!StaticStash.InstantActions.Has(entity))
                return;

            ref var instantActions = ref StaticStash.InstantActions.Get(entity);

            foreach (var instantActionEntity in instantActions.List) {
                StaticStash.Active.Remove(instantActionEntity);

                if (StaticStash.Duration.Has(instantActionEntity)) {
                    ref var duration = ref StaticStash.Duration.Get(instantActionEntity);
                    duration.Elapsed = 0;
                    duration.PercentElapsed = 0;
                }

                if (StaticStash.LerpingMovementOvershootTimer.Has(instantActionEntity)) {
                    ref var duration = ref StaticStash.LerpingMovementOvershootTimer.Get(instantActionEntity);
                    duration.Elapsed = 0;
                }

                if (StaticStash.TargetEntity.Has(instantActionEntity)) {
                    ref var targetEntity = ref StaticStash.TargetEntity.Get(instantActionEntity);
                    targetEntity.EntitySet = false;
                }
            }
        }

        private static void ResetUsageParts(Entity entity)
        {
            if (!StaticStash.UsageParts.Has(entity))
                return;

            ref var usageParts = ref StaticStash.UsageParts.Get(entity);

            AbilityUsageEntries.Clear();

            foreach (var entry in usageParts.UsagePartActivated.Keys)
                AbilityUsageEntries.Add(entry);

            foreach (var entry in AbilityUsageEntries)
                usageParts.UsagePartActivated[entry] = false;
        }

        private static void ResetUsageProgress(Entity entity)
        {
            if (!StaticStash.UsageProgress.Has(entity))
                return;

            ref var usageProgress = ref StaticStash.UsageProgress.Get(entity);

            StaticStash.Active.Remove(usageProgress.Entity);

            if (StaticStash.Duration.Has(usageProgress.Entity)) {
                ref var duration = ref StaticStash.Duration.Get(usageProgress.Entity);
                duration.Elapsed = 0;
                duration.PercentElapsed = 0;
            }
        }

        private static void ResetForwardConditions(Entity entity)
        {
            if (!StaticStash.ForwardConditions.Has(entity))
                return;

            ref var conditions = ref StaticStash.ForwardConditions.Get(entity);
            ref var toMeet = ref StaticStash.ConditionsToMeet.Get(entity);

            toMeet.Remaining = toMeet.Total;

            ResetConditionFulfilled(conditions.List);
        }

        private static void ResetCancelConditions(Entity entity)
        {
            if (!StaticStash.CancelConditions.Has(entity))
                return;

            ref var conditions = ref StaticStash.CancelConditions.Get(entity);
            ref var toMeet = ref StaticStash.CancelConditionsToMeet.Get(entity);

            toMeet.Remaining = conditions.List.Length;

            ResetConditionFulfilled(conditions.List);
        }

        private static void ResetConditionFulfilled(Entity[] conditionEntities)
        {
            foreach (var conditionEntity in conditionEntities) {
                StaticStash.Active.Remove(conditionEntity);

                ref var fulfilled = ref StaticStash.ConditionFulfilled.Get(conditionEntity);
                fulfilled.Value = 0;
            }
        }
    }
}