using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_SecondaryAttackPressedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_SecondaryAttack_IsPressed> _secondaryAttackIsPressed;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckSecondaryAttackPressed, Active>();
            _secondaryAttackIsPressed = World.GetStash<PlayerInput_SecondaryAttack_IsPressed>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                // Check if the user entity has the SecondaryAttack pressed component
                if (_secondaryAttackIsPressed.Has(user.Entity))
                {
                    ref var secondaryAttackPressed = ref _secondaryAttackIsPressed.Get(user.Entity);

                    // If secondary attack is pressed, fulfill the condition
                    if (secondaryAttackPressed.Value)
                    {
                        ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                        conditionFulfilled.Value++;
                    }
                }
            }
        }
    }
}
