using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitializeDamagableSystem : ISystem
    {
        private Stash<Damagable> _damagable;
        private Stash<DamageApplied> _damageApplied;
        private Filter _filter;
        private Stash<Health> _health;
        private Stash<Request_InitializeDamagable> _initializeDamagable;
        private Stash<MaxHealth> _maxHealth;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_InitializeDamagable>();
            _initializeDamagable = World.GetStash<Request_InitializeDamagable>();
            _damagable = World.GetStash<Damagable>();
            _health = World.GetStash<Health>();
            _maxHealth = World.GetStash<MaxHealth>();
            _damageApplied = World.GetStash<DamageApplied>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var initDamagable = ref _initializeDamagable.Get(entity);

                var damagableEntity = initDamagable.Instance.Initialize(World);

                _damagable.Set(damagableEntity, new Damagable {Instance = initDamagable.Instance,});
                _health.Set(damagableEntity, new Health {Value = initDamagable.Instance.Health,});
                _maxHealth.Set(damagableEntity, new MaxHealth {Value = initDamagable.Instance.Health,});
                _damageApplied.Add(damagableEntity);

                foreach (var healthPercentAction in initDamagable.Instance.HealthPercentActions)
                    healthPercentAction.Ability.Create(damagableEntity, World);

                entity.CompleteRequest();
            }
        }
    }
}