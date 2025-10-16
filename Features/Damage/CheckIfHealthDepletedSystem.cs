using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CheckIfHealthDepletedSystem : ISystem
    {
        private Stash<Dead> _dead;
        private Stash<DiedNow> _diedNow;
        private Filter _filter;
        private Stash<Health> _health;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Health>().Without<Dead>().Build();
            _health = World.GetStash<Health>();
            _dead = World.GetStash<Dead>();
            _diedNow = World.GetStash<DiedNow>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var targetHealth = ref _health.Get(entity);

                if (targetHealth.Value <= 0) {
                    _dead.Add(entity);
                    _diedNow.Add(entity);
                }
            }
        }
    }
}