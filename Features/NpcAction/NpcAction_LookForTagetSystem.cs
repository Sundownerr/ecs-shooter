using System.Collections.Generic;
using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.AbilityComponents;
using Game.Components;
using Game.NpcComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NpcAction_LookForTagetSystem : ISystem
    {
        private Filter _filter;
        private Filter _playerFilter;
        private Stash<UserEntity> _userEntityStash;
        private Stash<Targets> _targetsStash;
        private Stash<Active> _activeStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcAction_LookForTarget, Active>();
            _playerFilter = Entities.With<Player>();
            _userEntityStash = World.GetStash<UserEntity>();
            _targetsStash = World.GetStash<Targets>();
            _activeStash = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntityStash.Get(entity);

                if (!_targetsStash.Has(user.Entity))
                {
                    ref var target = ref _targetsStash.Add(user.Entity);
                    target.List = new List<Entity>();
                }

                ref var npcTarget = ref _targetsStash.Get(user.Entity);


                var playerEntity = _playerFilter.First();
                npcTarget.List.Add(playerEntity);

                _activeStash.Remove(entity);
            }
        }
    }
}
