using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class ProviderActivatorManager
    {
        private static readonly List<IProviderActivator> _pendingActivators = new();

        private static bool _levelLoaded;

        public static void NotifyLevelLoaded()
        {
            _levelLoaded = true;

            foreach (var activator in _pendingActivators)
                activator.ActivateProvider();

            _pendingActivators.Clear();
        }
        
        public static void NotifyLevelUnloaded()
        {
            _levelLoaded = false;
            _pendingActivators.Clear();
        }

        public static void Register(IProviderActivator activator)
        {
            if (_levelLoaded)
                activator.ActivateProvider();
            else
                _pendingActivators.Add(activator);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void ClearOnReload()
        {
            _pendingActivators.Clear();
            _levelLoaded = false;
        }
    }
}