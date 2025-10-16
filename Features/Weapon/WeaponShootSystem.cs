using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponShootSystem : ISystem
    {
        private Filter _filter;
        private Stash<Weapon> _weaponStash; 

        public World World { get; set; }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<Weapon>()
                .With<WeaponTriggerPulled>()
                .With<TimerCompleted>()
                .Build();

            _weaponStash = World.GetStash<Weapon>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var weapon = ref _weaponStash.Get(entity);
                
                if (!weapon.CanShoot)
                    continue;
                
                foreach (var abilityProvider in weapon.Instance.OnShootAbility)
                    abilityProvider.Activate();

                weapon.CanShoot = false;
            }
        }
    }
}