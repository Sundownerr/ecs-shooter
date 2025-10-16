using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HybridShootSystem : ISystem
    {
        private Stash<Event_WeaponHit> _eventRegisterWeaponHit;
        private Filter _filter;
        private Stash<HybridParticleGameObjectShooting> _hybridParticleGameObjectShooting;
        private Stash<Weapon> _weapon;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<HybridParticleGameObjectShooting>();
            _hybridParticleGameObjectShooting = World.GetStash<HybridParticleGameObjectShooting>();
            _weapon = World.GetStash<Weapon>();
            _eventRegisterWeaponHit = World.GetStash<Event_WeaponHit>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var shooting = ref _hybridParticleGameObjectShooting.Get(entity);
                ref var weapon = ref _weapon.Get(entity);

                var particles = new NativeArray<ParticleSystem.Particle>(100, Allocator.Temp);
                var existingParticles = shooting.ParticleSystem.GetParticles(particles);

                foreach (var (key, value) in shooting.Projectiles)
                    shooting.ParticleAlive[key] = false;

                for(var i = 0; i < existingParticles; i++) {
                    var particleSeed = particles[i].randomSeed;

                    if (!shooting.Projectiles.ContainsKey(particleSeed)) {
                        var go = Object.Instantiate(shooting.Prefab);
                        shooting.Projectiles.Add(particleSeed, go.transform);
                        shooting.ParticleAlive.Add(particleSeed, false);
                    }

                    shooting.ParticleAlive[particleSeed] = true;

                    shooting.Projectiles[particleSeed].position = particles[i].position;
                    shooting.Projectiles[particleSeed].localRotation = Quaternion.LookRotation(particles[i].velocity);
                }

                foreach (var (particleSeed, particleAlive) in shooting.ParticleAlive) {
                    if (!particleAlive)
                        shooting.DeadParticle.Add(particleSeed);
                }

                foreach (var particleSeed in shooting.DeadParticle) {
                    _eventRegisterWeaponHit.CreateEvent(new Event_WeaponHit {
                        Position = shooting.Projectiles[particleSeed].position,
                        WeaponInstance = weapon.Instance,
                    });

                    Object.Destroy(shooting.Projectiles[particleSeed].gameObject);

                    // Debug.Log("Destroying particle");

                    shooting.Projectiles.Remove(particleSeed);
                    shooting.ParticleAlive.Remove(particleSeed);
                }

                shooting.DeadParticle.Clear();
            }
        }
    }
}