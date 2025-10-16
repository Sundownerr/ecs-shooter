using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponCooldownCompletionSystem : ISystem
    {
        private Filter _cooldownFinishedFilter;
        private Stash<Weapon> _weaponStash;

        public World World { get; set; }

        public void Dispose() { }

        public void OnAwake()
        {
            _cooldownFinishedFilter = World.Filter
                .With<Weapon>()
                .With<TimerCompleted>()
                .Without<Reloading>()
                .Build();

            _weaponStash = World.GetStash<Weapon>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _cooldownFinishedFilter) {
                ref var weapon = ref _weaponStash.Get(entity);

                if (weapon.CanShoot)
                    continue;
                
                weapon.CanShoot = true;
            }
        }
    }
}