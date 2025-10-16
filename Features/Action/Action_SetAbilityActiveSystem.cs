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
    public sealed class Action_SetAbilityActiveSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<ParentEntity> _parentEntity;
        private Stash<SetAbilityActivated> _setAbilityActivated;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<SetAbilityActivated, Active>();

            // Initialize stashes
            _parentEntity = World.GetStash<ParentEntity>();
            _setAbilityActivated = World.GetStash<SetAbilityActivated>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parent = ref _parentEntity.Get(entity);
                ref var setAbilityActivated = ref _setAbilityActivated.Get(entity);

                var targetAbility = setAbilityActivated.Config._targetType is TargetType.Self ?
                    parent.Entity :
                    setAbilityActivated.Config.Other.Entity;

                // Debug.Log($"Set ability active: {setAbilityActivated.Config.Activated} for {setAbilityActivated.Config.Other.name}");

                targetAbility.SetAbilityActive(setAbilityActivated.Config.Activated);

                _active.Remove(entity);
            }
        }
    }
}
