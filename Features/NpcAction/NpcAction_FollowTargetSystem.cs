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
    public sealed class NpcAction_FollowTargetSystem : ISystem
    {
        private Filter _filter;
        private Stash<UserEntity> _userEntityStash;
        private Stash<NpcAction_FollowTarget> _npcActionFollowTargetStash;
        private Stash<NpcState_FollowTarget> _npcStateFollowTargetStash;
        private Stash<Active> _activeStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcAction_FollowTarget, Active>();
            _userEntityStash = World.GetStash<UserEntity>();
            _npcActionFollowTargetStash = World.GetStash<NpcAction_FollowTarget>();
            _npcStateFollowTargetStash = World.GetStash<NpcState_FollowTarget>();
            _activeStash = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntityStash.Get(entity);
                ref var action = ref _npcActionFollowTargetStash.Get(entity);

                ref var followTarget = ref _npcStateFollowTargetStash.Add(user.Entity);
                followTarget.MinDistance = action.MinDistance;

                _activeStash.Remove(entity);
            }
        }
    }
}
