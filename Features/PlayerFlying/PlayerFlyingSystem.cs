using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerFlyingSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() =>
            _filter = Entities.With<PlayerConfig, VerticalVelocity, PlayerInput_Jump_IsPressed>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var verticalVelocity = ref entity.GetComponent<VerticalVelocity>();
                ref var playerConfig = ref entity.GetComponent<PlayerConfig>();
                ref var jumpPressed = ref entity.GetComponent<PlayerInput_Jump_IsPressed>();

                if (jumpPressed.Value)
                    if (verticalVelocity.Value < 150)
                        verticalVelocity.Value += playerConfig.FlyingAscendSpeed * deltaTime;
            }
        }
    }
}