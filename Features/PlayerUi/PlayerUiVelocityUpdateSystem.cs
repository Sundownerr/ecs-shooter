using EcsMagic.PlayerComponenets;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUiVelocityUpdateSystem : ISystem
    {
        private readonly PlayerUIProvider _playerUIProvider;
        private Filter _filter;

        public PlayerUiVelocityUpdateSystem(ServiceLocator serviceLocator)
        {
            _playerUIProvider = serviceLocator.Get<UiService>().PlayerUi();
        }

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<Player>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var player = ref entity.GetComponent<Player>();

                var velocity = player.Instance.Rigidbody.velocity;

                _playerUIProvider.VelocityText.text = $"Speed: {velocity.magnitude:F1}";
            }
        }
    }
}