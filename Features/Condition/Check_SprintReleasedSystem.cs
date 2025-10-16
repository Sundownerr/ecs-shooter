using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_SprintReleasedSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_Sprint_WasReleased> _sprintWasReleased;
        private Stash<UserEntity> _userEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckSprintReleased, Active>();
            _sprintWasReleased = World.GetStash<PlayerInput_Sprint_WasReleased>();
            _userEntity = World.GetStash<UserEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                if (!_sprintWasReleased.Has(user.Entity))
                    continue;

                ref var sprintWasReleased = ref _sprintWasReleased.Get(user.Entity);

                if (!sprintWasReleased.Value)
                    continue;

                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }
    }
}
