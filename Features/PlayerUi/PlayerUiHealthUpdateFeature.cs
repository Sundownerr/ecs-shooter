using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUiHealthUpdateFeature : ISystem
    {
        private readonly PlayerUIProvider _playerUIProvider;
        private Filter _filter;

        public PlayerUiHealthUpdateFeature(ServiceLocator serviceLocator)
        {
            _playerUIProvider = serviceLocator.Get<UiService>().PlayerUi();
        }

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<Player, Health>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var health = ref entity.GetComponent<Health>();
                ref var maxHealth = ref entity.GetComponent<MaxHealth>();

                _playerUIProvider.HealthText.text = $"{health.Value}/{maxHealth.Value}";
                _playerUIProvider.HealthFill.fillAmount = (float) health.Value / maxHealth.Value;
            }
        }
    }
}