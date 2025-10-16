using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IParticleHitSubscriber
    {
        void OnParticleHit(GameObject hitGameObject, List<ParticleCollisionEvent> collisionEvents);
    }

    public class ParticleHitListener : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;
        private readonly List<ParticleCollisionEvent> _collisionEvents = new();
        private readonly List<IParticleHitSubscriber> _subscribers = new();

        private void OnParticleCollision(GameObject other)
        {
            _collisionEvents.Clear();
            var totalCollisions = ParticleSystem.GetCollisionEvents(other, _collisionEvents);

            for(var i = 0; i < totalCollisions; i++) {
                foreach (var subscriber in _subscribers)
                    subscriber.OnParticleHit(other, _collisionEvents);
            }
        }

        public void AddSubscriber(IParticleHitSubscriber subscriber) =>
            _subscribers.Add(subscriber);
    }
}