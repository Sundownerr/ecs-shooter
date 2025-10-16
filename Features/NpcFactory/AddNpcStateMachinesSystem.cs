using System.Collections.Generic;
using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddNpcStateMachinesSystem : ISystem
    {
        private Stash<Event_NpcCreated> _eventNpcCreated;
        private Filter _filter;
        private Stash<StateMachinesList> _stateMachines;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_NpcCreated>();

            _eventNpcCreated = World.GetStash<Event_NpcCreated>();
            _stateMachines = World.GetStash<StateMachinesList>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var evt = ref _eventNpcCreated.Get(entity);

                if (evt.Instance.StateMachines.Count == 0)
                    continue;

                ref var stateMachines = ref _stateMachines.Add(evt.Entity);
                stateMachines.List = new List<Entity>();

                foreach (var stateMachineProvider in evt.Instance.StateMachines)
                    stateMachines.List.Add(stateMachineProvider.Create(evt.Entity, World));
            }
        }
    }
}