using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateStateMachineExitTimeSystem : ISystem
    {
        private Filter _filter;
        private Stash<StateMachineExitTime> _stateMachineExitTime;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<StateMachineExitTime>();
            _stateMachineExitTime = World.GetStash<StateMachineExitTime>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var stateMachineExitTime = ref _stateMachineExitTime.Get(entity);

                if (stateMachineExitTime.Remaining > 0)
                    stateMachineExitTime.Remaining -= deltaTime;
            }
        }
    }
}
