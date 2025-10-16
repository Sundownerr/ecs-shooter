using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_SprintPressedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_Sprint_IsPressed> _sprintIsPressed;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckSprintPressed, Active>();
            _sprintIsPressed = World.GetStash<PlayerInput_Sprint_IsPressed>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                // Check if the user entity has the Sprint pressed component
                if (_sprintIsPressed.Has(user.Entity))
                {
                    ref var sprintPressed = ref _sprintIsPressed.Get(user.Entity);

                    // If sprint is pressed, fulfill the condition
                    if (sprintPressed.Value)
                    {
                        ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                        conditionFulfilled.Value++;
                    }
                }
            }
        }
    }
}
