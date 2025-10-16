using DG.Tweening;
using Game.Components;
using Game.Features;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CrosshairOnShootSystem : ISystem
    {
        private readonly PlayerUIProvider _playerUi;
        private Filter _filter;

        public CrosshairOnShootSystem(ServiceLocator serviceLocator)
        {
            _playerUi = serviceLocator.Get<UiService>().PlayerUi();
        }

        public void Dispose() { }

        public void OnAwake() =>
            _filter = Entities.With<PlayerTag, Weapon, WeaponTriggerPulled, TimerCompleted>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                _playerUi.PlayCrosshairShootAnimation();
            }
        }
    }
}