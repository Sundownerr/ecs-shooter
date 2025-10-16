using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkAbilitiesDestroySystem : ISystem
    {
        private Filter _filter;
        private Stash<AbilitiesList> _abilities;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WillBeDestroyed, AbilitiesList>();
            _abilities = World.GetStash<AbilitiesList>();
            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var abilities = ref _abilities.Get(entity);

                foreach (var ability in abilities.List)
                    _willBeDestroyed.Add(ability);
            }
        }
    }
}
