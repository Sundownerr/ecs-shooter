using System;
using System.Collections.Generic;
using Game.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class StateMachineProvider : EntityProvider
    {
        [Space]
        [TabGroup("$StateTabName")]
        [LabelText(" ")]
        [OnValueChanged(nameof(UpdateTransitions), true)]
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true)]
        public List<string> States;

        [TabGroup("$TransitionsTabName")]
        [LabelText(" ")]
        [OnValueChanged(nameof(UpdateTransitions), true)]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = true, ShowItemCount = false, Expanded = true)]
        public List<TransitionConfig> Transitions;

        [TabGroup("$EnterActionsTabName")]
        [LabelText(" ")]
        [OnValueChanged(nameof(UpdateTransitions), true)]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false, Expanded = true)]
        public List<StateEnterActions> EnterActions;

        [NonSerialized]
        public bool Created;

        private string StateTabName() => $"States ({States.Count.ToString()})";
        private string TransitionsTabName() => $"Transitions ({Transitions.Count.ToString()})";
        private string EnterActionsTabName() => $"State Enter Actions ({EnterActions.Count.ToString()})";

        [PropertyOrder(-1)]
        [Button]
        private void UpdateTransitions()
        {
            if (Transitions != null)
                foreach (var transition in Transitions)
                    transition.BrainProvider = this;

            if (EnterActions != null)
                foreach (var enterAction in EnterActions)
                    enterAction.BrainProvider = this;
        }

        public void ChangeToInitialState()
        {
            if (StaticStash.ChangeState.Has(Entity)) {
                ref var changeState = ref StaticStash.ChangeState.Get(Entity);
                changeState.NextState = States[0];
                changeState.TransitionEntity = default;
            }
            else {
                ref var initialState = ref StaticStash.ChangeState.Add(Entity);
                initialState.NextState = States[0];
            }
        }
    }
}