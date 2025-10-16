using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_DamageSystem : ISystem
    {
        private readonly DamageInstanceService _damageInstanceService;
        private Stash<Active> _active;
        private Stash<ApplyDamage> _applyDamage;
        private Filter _filter;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public Action_DamageSystem(ServiceLocator serviceLocator)
        {
            _damageInstanceService = serviceLocator.Get<DamageInstanceService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ApplyDamage, Active>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _applyDamage = World.GetStash<ApplyDamage>();
            _active = World.GetStash<Active>();
            _userEntity = World.GetStash<UserEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                ref var damage = ref _applyDamage.Get(entity);
                ref var user = ref _userEntity.Get(entity);

                switch (damage.Config.TargetType) {
                    case TargetType.Self:
                        _damageInstanceService.AddDamageInstance(user.Entity,user.Entity, damage.Config.Value);
                        break;

                    case TargetType.Target:
                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            foreach (var target in targets.List)
                                _damageInstanceService.AddDamageInstance(user.Entity, target, damage.Config.Value);
                        }

                        break;

                    case TargetType.Other:
                        // Handle Other case (currently same as Target)
                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            foreach (var target in targets.List)
                                _damageInstanceService.AddDamageInstance(user.Entity, target, damage.Config.Value);
                        }

                        break;
                }

                _active.Remove(entity);
            }
        }
    }
}