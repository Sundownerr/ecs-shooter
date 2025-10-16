using System.Collections.Generic;
using Game.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Systems
{
    public class DamageInstanceService
    {
        private Stash<DamageDealer> _damageDealer;
        private Stash<Health> _health;
        private World _world;
        public readonly List<DamageInstance> DamageInstances = new();

        public void Initialize(World world)
        {
            _world = world;
            _health = world.GetStash<Health>();
            _damageDealer = world.GetStash<DamageDealer>();
        }

        public void AddDamageInstance(Entity dealer, Entity target, int damage)
        {
            if (_world.IsDisposed(target))
                return;

            if (!_health.Has(target))
                return;

            var damageDealerType = DamageDealerType.None;

            if (!_world.IsDisposed(dealer) && _damageDealer.Has(dealer)) {
                ref var damageDealer = ref _damageDealer.Get(dealer);
                damageDealerType = damageDealer.Type;
            }

            DamageInstances.Add(new DamageInstance {
                Target = target,
                Damage = damage,
                DamageDealerType = damageDealerType,
            });
        }
        
        public void ClearDamageInstances()
        {
            DamageInstances.Clear();
        }
    }
}