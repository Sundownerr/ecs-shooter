using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CheckCancelConditionsMetSystem : ISystem
    {
        private Filter _filter;
        private Stash<CancelConditionsToMeet> _cancelConditionsToMeet;
        private Stash<Cancelled> _cancelled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<CancelConditionsToMeet>().Build();
            _cancelConditionsToMeet = World.GetStash<CancelConditionsToMeet>();
            _cancelled = World.GetStash<Cancelled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var cancelConditionsToMeet = ref _cancelConditionsToMeet.Get(entity);

                if (cancelConditionsToMeet.Remaining <= 0)
                    _cancelled.Add(entity);
            }
        }
    }
}
