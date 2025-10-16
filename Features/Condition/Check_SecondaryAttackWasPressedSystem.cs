using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_SecondaryAttackWasPressedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_SecondaryAttack_WasPressed> _secondaryAttackWasPressed;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckSecondaryAttackWasPressed, Active>();
            _secondaryAttackWasPressed = World.GetStash<PlayerInput_SecondaryAttack_WasPressed>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                if (!_secondaryAttackWasPressed.Has(user.Entity))
                    continue;

                ref var secondaryAttackWasPressed = ref _secondaryAttackWasPressed.Get(user.Entity);

                if (!secondaryAttackWasPressed.Value)
                    continue;

                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }
    }
}
