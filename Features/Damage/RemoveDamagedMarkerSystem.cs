using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveDamagedMarkerSystem : ISystem
    {
        private Filter _filter;
        private Stash<HasBeenDamaged> _hasBeenDamaged;
        private Stash<DamageApplied> _damageApplied;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<HasBeenDamaged, DamageApplied>();
            
            _hasBeenDamaged = World.GetStash<HasBeenDamaged>();
            _damageApplied = World.GetStash<DamageApplied>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                _hasBeenDamaged.Remove(entity);
                
                ref var damageApplied = ref _damageApplied.Get(entity);
                damageApplied.Total = 0;
            }
        }
    }
}