using Game;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace SDW.EcsMagic.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ClearTriggerNotificationsSystem : IFixedSystem
    {
        private Filter _enterFilter;
        private Filter _exitFilter;
        private Stash<NotifyTriggerEnter> _notifyTriggerEnter;
        private Stash<NotifyTriggerExit> _notifyTriggerExit;
        private Stash<NotifyTriggerStay> _notifyTriggerStay;
        private Filter _stayFilter;

        public void Dispose() { }

        public void OnAwake()
        {
            _enterFilter = Entities.With<NotifyTriggerEnter>();
            _exitFilter = Entities.With<NotifyTriggerExit>();
            _stayFilter = Entities.With<NotifyTriggerStay>();

            _notifyTriggerEnter = World.GetStash<NotifyTriggerEnter>();
            _notifyTriggerExit = World.GetStash<NotifyTriggerExit>();
            _notifyTriggerStay = World.GetStash<NotifyTriggerStay>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _enterFilter)
                _notifyTriggerEnter.Remove(entity);

            foreach (var entity in _exitFilter)
                _notifyTriggerExit.Remove(entity);

            foreach (var entity in _stayFilter)
                _notifyTriggerStay.Remove(entity);
        }
    }
}