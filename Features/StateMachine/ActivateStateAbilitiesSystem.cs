using Game.AbilityComponents;
using Game.Components;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ActivateStateAbilitiesSystem : ISystem
    {
        private Stash<AbilitiesList> _abilities;
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NeedsActivation, AbilitiesList>();
            _abilities = World.GetStash<AbilitiesList>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var abilities = ref _abilities.Get(entity);

                foreach (var ability in abilities.List)
                    ability.ActivateAbility();
            }
        }
    }
}