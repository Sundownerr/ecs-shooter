using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using SDW.EcsMagic.Triggers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public struct Trigger_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public TriggerInteractionType InteractionType;

        [HideLabel]
        public TriggerProvider TriggerInstance;

        public void AddTo(Entity entity)
        {
            switch (InteractionType)
            {
                case TriggerInteractionType.Enter:
                    StaticStash.TriggerConditionEnter.Add(entity);
                    TriggerInstance.EnterSubscribers.Add(entity);
                    break;

                case TriggerInteractionType.Stay:
                    StaticStash.TriggerConditionStay.Add(entity);
                    TriggerInstance.StaySubscribers.Add(entity);
                    break;

                case TriggerInteractionType.Exit:
                    StaticStash.TriggerConditionExit.Add(entity);
                    TriggerInstance.ExitSubscribers.Add(entity);
                    break;
            }
        }
    }
}
