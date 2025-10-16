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
    public sealed class AddNpcAbilitySystem : ISystem
    {
        private Stash<AbilitiesList> _abilities;
        private Stash<Event_NpcCreated> _eventNpcCreated;
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_NpcCreated>();

            _eventNpcCreated = World.GetStash<Event_NpcCreated>();
            _abilities = World.GetStash<AbilitiesList>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var evt = ref _eventNpcCreated.Get(entity);

                if (evt.Instance.Abilities.Count == 0)
                    continue;

                ref var abilities = ref _abilities.Add(evt.Entity);
                abilities.List = new List<Entity>();

                foreach (var abilityProvider in evt.Instance.Abilities)
                    abilities.List.Add(abilityProvider.Create(evt.Entity, World));
            }
        }
    }
}