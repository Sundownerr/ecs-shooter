using System;
using System.Collections.Generic;
using Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TransitionConfig
    {
        [Flags]
        public enum SettingsType
        {
            Condition = 1 << 0, Action = 1 << 1, ExitTime = 1 << 2,
        }

        private static readonly List<string> emptyList = new();

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 1)]
        [ValueDropdown(nameof(BrainStates))]
        [HorizontalGroup("Main", MaxWidth = 0.7f, PaddingLeft = 0.01f)] [HideLabel]
        public string From;

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 1)]
        [ValueDropdown(nameof(BrainStates))]
        [HorizontalGroup("Main", MaxWidth = 0.7f, PaddingRight = 0.01f)]
        [LabelText(" ", Icon = SdfIconType.ArrowRight)]
        [LabelWidth(20)]
        public string To;
        
        [FoldoutGroup("Details", Expanded = false, GroupName = "$GetGroupName")]
        [ShowIf(nameof(HaveCondition))]
        [LabelText("When", SdfIconType.QuestionCircleFill)]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 0)]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false,
            Expanded = true)]
        public List<AbilityConditionWrapper> NewConditions;

        [FoldoutGroup("Details")]
        [BoxGroup("Details/ex", ShowLabel = false)]
        [SuffixLabel("sec", Overlay = true)]
        [HorizontalGroup("Details/ex/0", PaddingLeft = 0.3f, PaddingRight = 0.3f)]
        [LabelWidth(60)]
        [ShowIf(nameof(HaveExitTime))]
        [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]
        [Min(0f)]
        [GUIColor(nameof(GetExitTimeColor))]
        public float ExitTime;

        [FoldoutGroup("Details")]
        [ShowIf(nameof(HaveAction))]
        [LabelText(" ")]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true)]
        public List<TransitionActionsConfig> Actions;

        [FoldoutGroup("Details")]
        [PropertySpace(SpaceBefore = 15, SpaceAfter = 5)]
        [HideLabel]
        public SettingsType Settings;

        [NonSerialized] public StateMachineProvider BrainProvider;

        private string GetGroupName() => Settings.ToString();

        private Color GetExitTimeColor() => ExitTime > 0f ? Color.white : Color.gray * 1.5f;
        private List<string> BrainStates() => BrainProvider == null ? emptyList : BrainProvider.States;

        public bool HaveCondition() => Settings.HasFlag(SettingsType.Condition);
        public bool HaveAction() => Settings.HasFlag(SettingsType.Action);
        public bool HaveExitTime() => Settings.HasFlag(SettingsType.ExitTime);
    }

    [Serializable]
    public class TransitionActionsConfig
    {
        [HorizontalGroup("t", MaxWidth = 0.3f)]
        [HideLabel] public TransitionActionType Type;

        [ShowIf(nameof(IsAction))]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false,
            Expanded = true)]
        [HideLabel]
        public AbilityUsage Actions;

        [ShowIf(nameof(IsNpcAction))]
        [LabelText(" ")]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false,
            Expanded = true)]
        public List<NpcAction> NpcActions;

        [ShowIf(nameof(IsAbilityProvdier))]
        [LabelText(" ")]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false,
            Expanded = true)]
        public List<AbilityProvider> AbilityProviders;
        private bool IsAction() => Type == TransitionActionType.Action;
        private bool IsAbilityProvdier() => Type == TransitionActionType.AbilityProvider;
        private bool IsNpcAction() => Type == TransitionActionType.NpcAction;
    }

    public enum TransitionActionType
    {
        Action = 0, NpcAction = 1, AbilityProvider,
    }
}