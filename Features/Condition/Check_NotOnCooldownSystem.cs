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
    public sealed class Check_NotOnCooldownSystem : ISystem
    {
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<CooldownParts> _cooldownParts;
        private Stash<CooldownFinished> _cooldownFinished;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NotOnCooldown, Active>();
            _parentEntity = World.GetStash<ParentEntity>();
            _cooldownParts = World.GetStash<CooldownParts>();
            _cooldownFinished = World.GetStash<CooldownFinished>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parent = ref _parentEntity.Get(entity);
                ref var cooldownParts = ref _cooldownParts.Get(parent.Entity);

                foreach (var cooldownEntity in cooldownParts.List)
                {
                    if (!_cooldownFinished.Has(cooldownEntity))
                        continue;

                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                    break;
                }
            }
        }
    }
}
