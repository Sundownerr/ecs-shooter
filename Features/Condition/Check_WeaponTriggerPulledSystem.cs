using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.StateMachineComponents;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_WeaponTriggerPulledSystem : ISystem
    {
        private Filter _filter;
        private Stash<CheckWeaponTriggerPulled> _checkWeaponTriggerPulled;
        private Stash<WeaponTriggerPulled> _weaponTriggerPulled;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckWeaponTriggerPulled, Active>();
            _checkWeaponTriggerPulled = World.GetStash<CheckWeaponTriggerPulled>();
            _weaponTriggerPulled = World.GetStash<WeaponTriggerPulled>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var checkWeaponTriggerPulled = ref _checkWeaponTriggerPulled.Get(entity);

                if (_weaponTriggerPulled.Has(checkWeaponTriggerPulled.Weapon))
                {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}
