using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateUserTargetSystem : ISystem
    {
        private Filter _filter;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<UserTarget, Active>();
            _targets = World.GetStash<Targets>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var user = ref _userEntity.Get(entity);

                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                ref var targets = ref _targets.Get(targetsProvider.Entity);

                targets.List.Clear();

                ref var userTarget = ref _targets.Get(user.Entity, out var exist);

                if (exist)
                    targets.List.Add(userTarget.List[0]);
            }
        }
    }
}