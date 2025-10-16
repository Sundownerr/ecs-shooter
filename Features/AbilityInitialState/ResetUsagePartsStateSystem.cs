using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetUsagePartsStateSystem : ISystem
    {
        private Filter _filter;
        private Stash<UsageConfigs> _usageConfigsStash;
        private Stash<UsageParts> _usagePartsStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<UsageParts, ShouldResetDurations>();
            _usageConfigsStash = World.GetStash<UsageConfigs>();
            _usagePartsStash = World.GetStash<UsageParts>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var usageConfigs = ref _usageConfigsStash.Get(entity);
                ref var usageParts = ref _usagePartsStash.Get(entity);

                foreach (var usageConfig in usageConfigs.List)
                    usageParts.UsagePartActivated[usageConfig] = false;
            }
        }
    }
}
