using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_ChangeYellowCubesSystem : ISystem
    {
        private Stash<AbilityChangeYellowCubes> _abilityChangeYellowCubes;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<Request_ChangeYellowCubes> _requestChangeYellowCubes;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityChangeYellowCubes, Active>();
            _abilityChangeYellowCubes = World.GetStash<AbilityChangeYellowCubes>();
            _active = World.GetStash<Active>();
            _requestChangeYellowCubes = World.GetStash<Request_ChangeYellowCubes>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var changeYellowCubes = ref _abilityChangeYellowCubes.Get(entity);

                _requestChangeYellowCubes.CreateRequest(new Request_ChangeYellowCubes {
                    Amount = changeYellowCubes.Delta,
                    AsGatheredOnLevel = changeYellowCubes.AsGatheredOnLevel,
                });

                _active.Remove(entity);
            }
        }
    }
}