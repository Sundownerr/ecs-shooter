using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ApplyDamageSystem : ISystem
    {
        private readonly DamageInstanceService _damageInstanceService;
        private Stash<Damagable> _damagable;
        private Stash<DamageApplied> _damageApplied;
        private Stash<HasBeenDamaged> _hasBeenDamaged;
        private Stash<Health> _health;
        private Stash<MaxHealth> _maxHealth;

        public ApplyDamageSystem(ServiceLocator serviceLocator)
        {
            _damageInstanceService = serviceLocator.Get<DamageInstanceService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _health = World.GetStash<Health>();
            _maxHealth = World.GetStash<MaxHealth>();
            _damagable = World.GetStash<Damagable>();
            _damageApplied = World.GetStash<DamageApplied>();
            _hasBeenDamaged = World.GetStash<HasBeenDamaged>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var damageInstance in _damageInstanceService.DamageInstances) {
                ref var damageApplied = ref _damageApplied.Get(damageInstance.Target);
                damageApplied.Total += damageInstance.Damage;

                ref var health = ref _health.Get(damageInstance.Target);

                if (health.Value > 0)
                    damageApplied.LastDamageDealer = damageInstance.DamageDealerType;
                
                health.Value -= damageInstance.Damage;

                if (!_hasBeenDamaged.Has(damageInstance.Target))
                    _hasBeenDamaged.Add(damageInstance.Target);

                if (!_damagable.Has(damageInstance.Target))
                    continue;

                ref var damagable = ref _damagable.Get(damageInstance.Target);
                ref var maxHealth = ref _maxHealth.Get(damageInstance.Target);
                var currentHealthPercent = (float) health.Value / maxHealth.Value;

                for(var i = damagable.Stage; i < damagable.Instance.HealthPercentActions.Count; i++) {
                    var action = damagable.Instance.HealthPercentActions[i];

                    if (action.TriggerPercent > currentHealthPercent) {
                        damagable.Stage = i + 1;
                        action.Ability.Activate();

                        // Debug.Log($"Health percent action {action.TriggerPercent} activated");
                    }
                }
            }
            
            _damageInstanceService.ClearDamageInstances();
        }
    }
}