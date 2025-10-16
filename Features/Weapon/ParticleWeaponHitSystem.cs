using Game.Providers;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ParticleWeaponHitSystem : ISystem
    {
        private readonly DamageInstanceService _damageInstanceService;
        private Stash<Event_WeaponHit> _eventRegisterWeaponHit;
        private Filter _filter;
        private Stash<Event_ParticleWeaponHit> _particleWeaponHit;
        private Stash<Weapon> _weapon;

        public ParticleWeaponHitSystem(ServiceLocator serviceLocator)
        {
            _damageInstanceService = serviceLocator.Get<DamageInstanceService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_ParticleWeaponHit>();
            _particleWeaponHit = World.GetStash<Event_ParticleWeaponHit>();
            _eventRegisterWeaponHit = World.GetStash<Event_WeaponHit>();
            _weapon = World.GetStash<Weapon>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var hit = ref _particleWeaponHit.Get(entity);

                foreach (var particleCollisionEvent in hit.CollisionEvents) {
                    if (particleCollisionEvent.colliderComponent == null)
                        continue;

                    _eventRegisterWeaponHit.CreateEvent(new Event_WeaponHit {
                        Position = particleCollisionEvent.intersection,
                        WeaponInstance = hit.WeaponInstance,
                    });

                    if (!particleCollisionEvent.colliderComponent.TryGetComponent<EntityProvider>(
                        out var entityProvider))
                        continue;

                    ref var weapon = ref _weapon.Get(hit.WeaponInstance.Entity);
                    
                    _damageInstanceService.AddDamageInstance(weapon.User,entityProvider.Entity, hit.WeaponInstance.Damage);
                }
            }
        }
    }
}