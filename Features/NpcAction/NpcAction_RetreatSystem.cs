using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.NpcComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NpcAction_RetreatSystem : ISystem
    {
        private Filter _filter;
        private Stash<UserEntity> _userEntityStash;
        private Stash<NpcAction_Retreat> _npcActionRetreatStash;
        private Stash<NpcState_Retreat> _npcStateRetreatStash;
        private Stash<Active> _activeStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcAction_Retreat, Active>();
            _userEntityStash = World.GetStash<UserEntity>();
            _npcActionRetreatStash = World.GetStash<NpcAction_Retreat>();
            _npcStateRetreatStash = World.GetStash<NpcState_Retreat>();
            _activeStash = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntityStash.Get(entity);
                ref var action = ref _npcActionRetreatStash.Get(entity);

                ref var state = ref _npcStateRetreatStash.Add(user.Entity);
                state.MinDistance = action.MinDistance;
                state.Radius = action.Radius;

                _activeStash.Remove(entity);
            }
        }
    }
}
