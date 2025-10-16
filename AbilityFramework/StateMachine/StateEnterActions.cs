using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public class StateEnterActions
    {
        private static readonly List<string> emptyList = new();
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 1)]
        [ValueDropdown(nameof(BrainStates))]
        [HorizontalGroup("Main")] [HideLabel]
        public string From;

        private List<string> BrainStates() => BrainProvider == null ? emptyList : BrainProvider.States;
        [NonSerialized] public StateMachineProvider BrainProvider;
        
        [LabelText(" ")]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true)]
        public List<TransitionActionsConfig> Actions;
    }
}