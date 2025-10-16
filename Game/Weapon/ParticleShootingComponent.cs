using System.Collections.Generic;
using Game.Systems;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class ParticleShootingComponent : WeaponShootingComponent, IParticleHitSubscriber, IProviderActivator
    {
        public ParticleHitListener ParticleHitListener;
        private Stash<Event_ParticleWeaponHit> _stash;

        private void Start()
        {
            ProviderActivatorManager.Register(this);
            ParticleHitListener.AddSubscriber(this);
        }

        public void OnParticleHit(GameObject hitGameObject, List<ParticleCollisionEvent> collisionEvents) =>
            _stash.CreateEvent(new Event_ParticleWeaponHit {
                WeaponInstance = _weaponProvider,
                CollisionEvents = collisionEvents,
            });

        public void ActivateProvider() =>
            _stash = World.Default.GetStash<Event_ParticleWeaponHit>();
    }
}