using System;
using Game.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class SpawnerProvider : EntityProvider
    {
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        public AbilityProvider[] AbilitiesToInitialize;

        [Space]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        [InlineEditor]
        [LabelText("On Activated", SdfIconType.SunFill)]
        public AbilityProvider[] OnActivateAbility;

        [Space]
        [Space]
        [LabelText("On Deactivated", SdfIconType.Sun)]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        [InlineEditor]
        public AbilityProvider[] OnDeactivateAbility;

        [NonSerialized]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowIndexLabels = false, ShowItemCount = false)]
        [InlineEditor]
        public AbilityProvider[] OnUsedAbility;

        public void Activate()
        {
            foreach (var abilityProvider in OnActivateAbility)
                abilityProvider.Activate();

            foreach (var abilityProvider in OnDeactivateAbility)
                abilityProvider.Deactivate();
        }

        public void Deactivate()
        {
            foreach (var abilityProvider in OnDeactivateAbility)
                abilityProvider.Activate();

            foreach (var abilityProvider in OnActivateAbility)
                abilityProvider.Deactivate();
        }

        public void Use()
        {
            foreach (var abilityProvider in OnUsedAbility)
                abilityProvider.Activate();
        }
    }
}