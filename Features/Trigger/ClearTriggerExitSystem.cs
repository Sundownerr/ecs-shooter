using Game;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace SDW.EcsMagic.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ClearTriggerExitSystem : IFixedSystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<Trigger>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var trigger = ref entity.GetComponent<Trigger>();
                trigger.Instance.Exit.Clear();
            }
        }
    }
}