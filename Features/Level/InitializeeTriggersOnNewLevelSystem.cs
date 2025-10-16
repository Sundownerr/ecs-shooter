using System.Collections.Generic;
using Game.Components;
using Scellecs.Morpeh;
using SDW.EcsMagic.Triggers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitializeeTriggersOnNewLevelSystem : ISystem
    {
        private Filter _filter;

        private List<TriggerProvider> _triggers = new();

        public void Dispose() { }

        public void OnAwake() =>
            _filter = Entities.With<Event_CompletedLoadingScene>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
              
                foreach (var triggerProvider in Object.FindObjectsOfType<TriggerProvider>())
                    triggerProvider.InitializeTrigger(World);
            }
        }
    }
}