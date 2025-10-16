using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_PrimaryAttackPressedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_PrimaryAttack_IsPressed> _primaryAttackIsPressed;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckPrimaryAttackPressed, Active>();
            _primaryAttackIsPressed = World.GetStash<PlayerInput_PrimaryAttack_IsPressed>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                // Check if the user entity has the PrimaryAttack pressed component
                if (_primaryAttackIsPressed.Has(user.Entity))
                {
                    ref var primaryAttackPressed = ref _primaryAttackIsPressed.Get(user.Entity);

                    // If primary attack is pressed, fulfill the condition
                    if (primaryAttackPressed.Value)
                    {
                        ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                        conditionFulfilled.Value++;
                    }
                }
            }
        }
    }
}
