using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkStateMachinesToDestroySystem : ISystem
    {
        private Filter _filter;
        private Stash<StateMachinesList> _stateMachinesList;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<StateMachinesList, WillBeDestroyed>();

            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
            _stateMachinesList = World.GetStash<StateMachinesList>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var stateMachines = ref _stateMachinesList.Get(entity);

                foreach (var stateMachine in stateMachines.List)
                    _willBeDestroyed.Add(stateMachine);
            }
        }
    }
}