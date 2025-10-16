using System;
using System.Collections.Generic;
using Ability;
using Ability.Targeting;
using EcsMagic.Abilities;
using Game.AbilityComponents;
using Game.Providers;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Flags]
    public enum AbilityProviderSettings
    {
        Targeting = 1 << 0,
        UseConditions = 1 << 1,
        Cooldown = 1 << 2,
        OneShot = 1 << 3,
    }

    public class AbilityProvider : EntityProvider
    {
        [LabelText("Don't Deactivate On Start")]
        public bool ActiveOnStart = true;
        public bool Debug;

        [Space]
        [EnumToggleButtons]
        [HideLabel]
        public AbilityProviderSettings Settings;

        
        [ShowIf(nameof(HaveTargeting))]
        [LabelText("Targets", SdfIconType.PeopleFill)]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityTargetWrapper> NewTarget;
        
        [Space]
        [Space]
        [ShowIf(nameof(HaveUseConditions))]
        [LabelText("Use When", SdfIconType.QuestionCircleFill)]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityConditionWrapper> NewUseConditions;
        

        [LabelText("Cooldown", SdfIconType.AlarmFill)]
        [ShowIf(nameof(HaveCooldown))]
        [Space]
        [Space]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityCooldownWrapper> NewCooldown;
      
        [LabelText("Cancel channel when", SdfIconType.LightningChargeFill)]
        [Space]
        [Space]
        [ShowIf(nameof(IsChannelUsage))]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityConditionWrapper> NewCancelWhen;

        [LabelText("When channel cancelled", SdfIconType.QuestionCircleFill)]
        [ShowIf(nameof(IsChannelUsage))]
        [Space]
        [Space]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityActionWrapper> OnUsageCancelled;

        [HideLabel]
        [BoxGroup("Usage", ShowLabel = false)]
        public AbilityUsage UsageConfig;

        [NonSerialized]
        public bool Created;

        public bool HaveTargeting() => Settings.HasFlag(AbilityProviderSettings.Targeting);
        public bool HaveUseConditions() => Settings.HasFlag(AbilityProviderSettings.UseConditions);
        public bool HaveCooldown() => Settings.HasFlag(AbilityProviderSettings.Cooldown);
        public bool OneShot() => Settings.HasFlag(AbilityProviderSettings.OneShot);
        public bool IsChannelUsage() => UsageConfig.Usage.Type == AbilityTimingType.Channel;

        public void SetCustomPosition(Vector3 position)
        {
            ref var ability = ref Entity.GetComponent<AbilityCustomData>();
            ability.CustomPosition = position;
        }

        [HideInEditorMode]
        [ButtonGroup("0")]
        [Button]
        [PropertyOrder(-1)]
        public void Activate() => Entity.ActivateAbility();

        [HideInEditorMode]
        [ButtonGroup("0")]
        [Button]
        [PropertyOrder(-1)]
        public void Deactivate() => Entity.DeactivateAbility();
    }
}