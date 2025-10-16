using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ApplyMoveSpeedToMovement : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() =>
            _filter = Entities.With<Player, MoveDirection, FloatStats>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var moveDirection = ref entity.GetComponent<MoveDirection>();
                ref var stats = ref entity.GetComponent<FloatStats>();

                moveDirection.Value *= stats.Value.ValueOf(Stat.MoveSpeed);
            }
        }
    }
}