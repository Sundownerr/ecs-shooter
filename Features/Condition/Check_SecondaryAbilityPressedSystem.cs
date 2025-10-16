using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_SecondaryAbilityPressedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_SecondaryAbility_IsPressed> _secondaryAbilityIsPressed;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckSecondaryAbilityPressed, Active>();
            _secondaryAbilityIsPressed = World.GetStash<PlayerInput_SecondaryAbility_IsPressed>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                // Check if the user entity has the SecondaryAbility pressed component
                if (_secondaryAbilityIsPressed.Has(user.Entity))
                {
                    ref var secondaryAbilityPressed = ref _secondaryAbilityIsPressed.Get(user.Entity);

                    // If the secondary ability is pressed, fulfill the condition
                    if (secondaryAbilityPressed.Value)
                    {
                        ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                        conditionFulfilled.Value++;
                    }
                }
            }
        }
    }
}
