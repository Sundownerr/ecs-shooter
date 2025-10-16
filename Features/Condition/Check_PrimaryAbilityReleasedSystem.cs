using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_PrimaryAbilityReleasedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_PrimaryAbility_WasReleased> _primaryAbilityWasReleased;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckPrimaryAbilityReleased, Active>();
            _primaryAbilityWasReleased = World.GetStash<PlayerInput_PrimaryAbility_WasReleased>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                if (!_primaryAbilityWasReleased.Has(user.Entity))
                    continue;

                ref var primaryAbilityWasReleased = ref _primaryAbilityWasReleased.Get(user.Entity);

                if (!primaryAbilityWasReleased.Value)
                    continue;

                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }
    }
}
