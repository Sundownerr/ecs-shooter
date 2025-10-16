using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Data;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUiManaUpdateFeature : ISystem
    {
        private readonly PlayerUIProvider _playerUIProvider;
        private Filter _filter;

        private Stash<FloatStats> _floatStats;

        public PlayerUiManaUpdateFeature(ServiceLocator serviceLocator)
        {
            _playerUIProvider = serviceLocator.Get<UiService>().PlayerUi();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, FloatStats>();
            _floatStats = World.GetStash<FloatStats>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var floatStats = ref _floatStats.Get(entity);

                var currentMana = floatStats.Value.ValueOf(Stat.Mana);
                var maxMana = floatStats.Value.ValueOf(Stat.MaxMana);

                _playerUIProvider.ManaText.text = $"{currentMana}/{maxMana}";
                _playerUIProvider.ManaFill.fillAmount = currentMana / maxMana;
            }
        }
    }
}