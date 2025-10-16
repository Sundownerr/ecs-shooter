using Game.Components;
using Scellecs.Morpeh;

namespace Game.Systems
{
    public static class LevelsEcsUtility
    {
        private static Stash<Request_CreateLevel> _requestCreateLevel;
        private static Stash<Event_LevelChanged> _eventLevelChanged;
        private static Stash<Trigger_ReactOn_LevelChanged> _triggerReactOnWorldChanged;
        private static Filter _reactorsFilter;

        public static void Initialize(World world)
        {
            _reactorsFilter = Entities.With<ReactOn_LevelChanged>();
            _triggerReactOnWorldChanged = world.GetStash<Trigger_ReactOn_LevelChanged>();
            _eventLevelChanged = world.GetStash<Event_LevelChanged>();
            _requestCreateLevel = world.GetStash<Request_CreateLevel>();
        }

        public static void NotifyLevelUnloaded()
        {
            _eventLevelChanged.CreateEvent();
            _reactorsFilter.TriggerReaction(_triggerReactOnWorldChanged);

            ProviderActivatorManager.NotifyLevelUnloaded();
        }

        public static void NotifyLoadingNewLevel(int levelIndex)
        {
            _requestCreateLevel.CreateRequest(new Request_CreateLevel {Index = levelIndex,});
            NotifyLevelUnloaded();
        }
    }
}