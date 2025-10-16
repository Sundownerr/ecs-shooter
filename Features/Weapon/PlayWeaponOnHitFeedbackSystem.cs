using Game.AbilityComponents;
using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayWeaponOnHitFeedbackSystem : ISystem
    {
        private Filter _filter;
        private Stash<Event_WeaponHit> _weaponHit;
        private Stash<AbilityCustomData> _abilityCustomData;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_WeaponHit>();
            _weaponHit = World.GetStash<Event_WeaponHit>();
            _abilityCustomData = World.GetStash<AbilityCustomData>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var request = ref _weaponHit.Get(entity);

                foreach (var abilityProvider in request.WeaponInstance.OnHitAbility)
                {
                    var ability = abilityProvider.Entity;

                    ref var customData = ref _abilityCustomData.Get(ability);
                    customData.CustomPosition = request.Position;

                    ability.ActivateAbility();
                }

                foreach (var abilityProvider in request.WeaponInstance.OnTriggerPulledAbility)
                {
                    var ability = abilityProvider.Entity;

                    ref var customData = ref _abilityCustomData.Get(ability);
                    customData.CustomPosition = request.Position;
                }

                foreach (var abilityProvider in request.WeaponInstance.OnTriggerReleasedAbility)
                {
                    var ability = abilityProvider.Entity;

                    ref var customData = ref _abilityCustomData.Get(ability);
                    customData.CustomPosition = request.Position;
                }
            }
        }
    }
}
