using System;
using System.Collections.Generic;
using Game.Components;
using Game.Providers;
using Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class DamagableProvider : EntityProvider, IProviderActivator
    {
        public bool DestroyGameObjectOnDeath = true;
        public int Health;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        public List<HealthPercentAction> HealthPercentActions;

        private void Awake() =>
            ProviderActivatorManager.Register(this);

        public void ActivateProvider() =>
            StaticStash.Request_InitializeDamagable
                .CreateRequest(new Request_InitializeDamagable {Instance = this,});

        [Serializable]
        public class HealthPercentAction
        {
            [GUIColor(nameof(GetColor))]
            [HideLabel] [Range(0f, 1f)] public float TriggerPercent = 1f;
            [HideLabel] [InlineEditor] public AbilityProvider Ability;

            private Color GetColor() =>
                Color.Lerp(Color.red, Color.green, TriggerPercent) * 2f;
        }
    }
}