using EcsMagic.NpcComponents;
using Game.Components;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CrosshairKillAnimationSystem : ISystem
    {
        private readonly PlayerUIProvider _playerUi;
        private Stash<DamageApplied> _damageApplied;
        private Filter _filter;

        public CrosshairKillAnimationSystem(ServiceLocator serviceLocator)
        {
            _playerUi = serviceLocator.Get<UiService>().PlayerUi();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Npc, DiedNow>();
            _damageApplied = World.GetStash<DamageApplied>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var damage = ref _damageApplied.Get(entity);

                if (damage.LastDamageDealer is DamageDealerType.Player)
                    _playerUi.PlayCrosshairKillAnimation();
            }
        }
    }
}