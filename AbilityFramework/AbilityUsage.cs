using System;
using System.Collections.Generic;
using Ability;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityUsage
    {
        [InlineProperty]
        [HideLabel]
        [OnValueChanged(nameof(ChangeEntries), true)]
        public AbilityTiming Usage;

        [ShowIf(nameof(IsInstantUsage))]
        [LabelText(" ", SdfIconType.LightningChargeFill)]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityActionWrapper> NewAbilityActions;

        [LabelText("Timeline")]
        [ShowIf(nameof(IsChannelUsage))]
        [Space]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityUsageEntry> UsageEntries;

        private void ChangeEntries()
        {
            if (IsInstantUsage())
                return;

            foreach (var abilityUsageEntry in UsageEntries)
                abilityUsageEntry.MaxTimeValue = Usage.Channel.Duration;
            // if (abilityUsageEntry.Type == AbilityUsageEntry.Use.Time)
            //     abilityUsageEntry.Value = Mathf.Clamp(abilityUsageEntry.Value, 0, Usage.Channel.Duration);
        }

        private bool IsChannelUsage() => Usage.Type == AbilityTimingType.Channel;

        private bool IsInstantUsage() => Usage.Type == AbilityTimingType.Instant;

        public void AddTo(Entity parentEntity, Entity userEntity, Entity targetProviderEntity, World world)
        {
            var usageEntity = world.CreateEntity();

            StaticStash.ParentEntity.Set(usageEntity, new ParentEntity {Entity = parentEntity,});

            StaticStash.UsageProgress.Set(parentEntity, new UsageProgress {Entity = usageEntity,});

            if (Usage.Type is AbilityTimingType.Channel) {
                StaticStash.Duration.Set(usageEntity, new Duration {Max = Usage.Channel.Duration,});
                StaticStash.UsageProgressPart.Set(usageEntity, new UsageProgressPart());

                var usageEntriesCount = UsageEntries.Count;

                if (usageEntriesCount <= 0)
                    return;
                
                StaticStash.UsageConfigs.Set(parentEntity, new UsageConfigs {List = UsageEntries,});

                ref var usageParts = ref StaticStash.UsageParts.Add(parentEntity);

                usageParts.UsagePartActivated = new Dictionary<AbilityUsageEntry, bool>(usageEntriesCount);
                usageParts.UsagePartActions = new Dictionary<AbilityUsageEntry, List<Entity>>(usageEntriesCount);

                foreach (var entry in UsageEntries) {
                    usageParts.UsagePartActions.Add(entry, new List<Entity>(entry.NewAbilityActions.Count));
                    usageParts.UsagePartActivated.Add(entry, false);
                    entry.AddToAbility(ref usageParts, world, parentEntity, userEntity, targetProviderEntity);
                }
            }
            else {
                if (NewAbilityActions.Count <= 0)
                    return;

                var instantActionsList = new List<Entity>(NewAbilityActions.Count);

                foreach (var useAction in NewAbilityActions) {
                    instantActionsList.Add(useAction.CreateEntity(world, parentEntity, userEntity, 
                        targetProviderEntity));
                }

                StaticStash.InstantActions.Set(parentEntity, new InstantActions {List = instantActionsList,});
            }
        }
    }
}