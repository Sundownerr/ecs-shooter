using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerWeaponTriggerPullSystem : ISystem
    {
        private Filter _filter;
        private Stash<WeaponTriggerPulled> _weaponTriggerPulled;
        private Stash<PlayerInput_PrimaryAttack_IsPressed> _playerInput_PrimaryAttack_IsPressed;
        private Stash<Weapon> _weapon;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ActiveWeapon, PlayerInput_PrimaryAttack_IsPressed>();
            _weaponTriggerPulled = World.GetStash<WeaponTriggerPulled>();
            _playerInput_PrimaryAttack_IsPressed = World.GetStash<PlayerInput_PrimaryAttack_IsPressed>();
            _weapon = World.GetStash<Weapon>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var shootKeyPressed = ref _playerInput_PrimaryAttack_IsPressed.Get(entity);
                ref var weapon = ref _weapon.Get(entity);
                var hasComponent = _weaponTriggerPulled.Has(entity);

                switch (shootKeyPressed.Value)
                {
                    case true when !hasComponent:
                        weapon.Instance.PullTrigger();
                        break;
                    case false when hasComponent:
                        weapon.Instance.ReleaseTrigger();
                        break;
                }
            }
        }
    }
}
