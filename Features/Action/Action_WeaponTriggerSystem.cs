using System;
using EcsMagic.Abilities;
using EcsMagic.Actions;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_WeaponTriggerSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<WeaponTriggerAction> _weaponTriggerAction;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WeaponTriggerAction, Active>();

            // Initialize stashes
            _weaponTriggerAction = World.GetStash<WeaponTriggerAction>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var weaponTrigger = ref _weaponTriggerAction.Get(entity);

                switch (weaponTrigger.Config.TriggerAction)
                {
                    case WeaponTriggerConfig.Action.Pull:
                        weaponTrigger.Config.Weapon.PullTrigger();
                        break;
                    case WeaponTriggerConfig.Action.Release:
                        weaponTrigger.Config.Weapon.ReleaseTrigger();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _active.Remove(entity);
            }
        }
    }
}
