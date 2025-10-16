using Unity.Collections;
using UnityEngine;

namespace Game
{
    public class ParticleCollisionPrototype : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float emissionInterval = 0.5f;
        [SerializeField] private float collisionCheckRadius = 0.1f;

        private float emissionTimer;
        private NativeArray<ParticleSystem.Particle> particles;

        private void Update()
        {
            // Emit particles at regular intervals
            emissionTimer += Time.deltaTime;

            if (emissionTimer >= emissionInterval) {
                _particleSystem.Emit(1);
                emissionTimer = 0f;
            }

            // Check for collisions and modify particles
            var particleCount = _particleSystem.particleCount;

            if (particleCount > 0) {
                // Get all particles
                if (!particles.IsCreated || particles.Length < particleCount) {
                    // Dispose of the old array if it exists
                    if (particles.IsCreated)
                        particles.Dispose();

                    // Create a new NativeArray with the appropriate size
                    particles = new NativeArray<ParticleSystem.Particle>(
                        particleCount, Allocator.Temp);
                }

                _particleSystem.GetParticles(particles);

                // Check each particle for collisions
                var particlesModified = false;

                for(var i = 0; i < particleCount; i++) {
                    var particle = particles[i];
                    var position = particle.position;

                    // Check for collision
                    if (Physics.CheckSphere(position, collisionCheckRadius)) {
                        // Stop particle and set lifetime to 0
                        particle.velocity = Vector3.zero;
                        particle.remainingLifetime = 0f;
                        particles[i] = particle;
                        particlesModified = true;
                    }
                }

                // Apply changes to particle system
                if (particlesModified)
                    _particleSystem.SetParticles(particles, particleCount);
            }
        }

        private void OnDisable()
        {
            // Ensure we dispose of the NativeArray when the component is disabled
            if (particles.IsCreated)
                particles.Dispose();
        }

        private void OnDestroy()
        {
            // Ensure we dispose of the NativeArray when the component is destroyed
            if (particles.IsCreated)
                particles.Dispose();
        }
    }
}