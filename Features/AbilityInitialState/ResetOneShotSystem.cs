using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetOneShotSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<AbilityState_Initial, OneShotAbility>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
                entity.DeactivateAbility();
        }
    }
}