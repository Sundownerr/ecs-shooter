using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetCancelConditionsToMeetSystem : ISystem
    {
        private Filter _filter;
        private Stash<CancelConditionsToMeet> _cancelConditionsToMeetStash;
        private Stash<CancelConditions> _cancelConditionsStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CancelConditionsToMeet>();
            _cancelConditionsToMeetStash = World.GetStash<CancelConditionsToMeet>();
            _cancelConditionsStash = World.GetStash<CancelConditions>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var toMeet = ref _cancelConditionsToMeetStash.Get(entity);
                ref var conditions = ref _cancelConditionsStash.Get(entity);
                toMeet.Remaining = conditions.List.Length;
            }
        }
    }
}
