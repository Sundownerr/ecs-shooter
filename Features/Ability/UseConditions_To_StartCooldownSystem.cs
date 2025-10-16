using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UseConditions_To_StartCooldownSystem : ISystem
    {
        private Stash<AbilityState_CheckUseConditions> _abilityStateCheckUseConditions;
        private Stash<AbilityState_StartCooldown> _abilityStateStartCooldown;
        private Stash<Active> _active;
        private Stash<Cancelled> _cancelled;
        private Stash<CooldownParts> _cooldownParts;
        private Filter _filter;
        private Stash<ForwardConditionsToMeet> _forwardConditionsToMeet;
        private Stash<PartsToComplete> _partsToComplete;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<AbilityState_CheckUseConditions>().Without<Cancelled>().Build();

            _abilityStateCheckUseConditions = World.GetStash<AbilityState_CheckUseConditions>();
            _cancelled = World.GetStash<Cancelled>();
            _forwardConditionsToMeet = World.GetStash<ForwardConditionsToMeet>();
            _abilityStateStartCooldown = World.GetStash<AbilityState_StartCooldown>();
            _partsToComplete = World.GetStash<PartsToComplete>();
            _cooldownParts = World.GetStash<CooldownParts>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                if (_forwardConditionsToMeet.Has(entity)) {
                    ref var conditionsToMeet = ref _forwardConditionsToMeet.Get(entity);

                    if (conditionsToMeet.Remaining > 0)
                        continue;
                }

                _abilityStateCheckUseConditions.Remove(entity);
                _abilityStateStartCooldown.Add(entity);

                ref var partsToComplete = ref _partsToComplete.Get(entity);

                if (!_cooldownParts.Has(entity)) {
                    partsToComplete.Value = 0;
                    continue;
                }

                ref var cooldownParts = ref _cooldownParts.Get(entity);
                partsToComplete.Value = cooldownParts.List.Count;

                foreach (var cooldown in cooldownParts.List)
                    _active.Add(cooldown);

                // Debug.Log("Check Use Conditions -> Start Cooldown");
            }
        }
    }
}