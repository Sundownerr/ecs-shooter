using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_ChangeComponentEnabledSystem : ISystem
    {
        private Filter _filter;
        private Stash<ChangeComponentEnabled> _changeComponentEnabled;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ChangeComponentEnabled, Active>();
            _changeComponentEnabled = World.GetStash<ChangeComponentEnabled>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var changeComponentEnabled = ref _changeComponentEnabled.Get(entity);

                if (changeComponentEnabled.Config.TargetComponent != null)
                {
                    // changeComponentEnabled.Config.TargetComponent.enabled = changeComponentEnabled.Config.Enabled;
                }

                _active.Remove(entity);
            }
        }
    }
}