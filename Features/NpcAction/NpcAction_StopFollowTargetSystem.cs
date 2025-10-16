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
    public sealed class NpcAction_StopFollowTargetSystem : ISystem
    {
        private Filter _filter;
        private Stash<UserEntity> _userEntityStash;
        private Stash<NpcState_StopFollowTarget> _npcStateStopFollowTargetStash;
        private Stash<Active> _activeStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcAction_StopFollowTarget, Active>();
            _userEntityStash = World.GetStash<UserEntity>();
            _npcStateStopFollowTargetStash = World.GetStash<NpcState_StopFollowTarget>();
            _activeStash = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntityStash.Get(entity);
                _npcStateStopFollowTargetStash.Add(user.Entity);
                _activeStash.Remove(entity);
            }
        }
    }
}
