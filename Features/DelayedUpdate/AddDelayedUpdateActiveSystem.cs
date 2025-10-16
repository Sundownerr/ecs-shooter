using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddDelayedUpdateActiveSystem : ISystem
    {
        private Filter _filter;
        private Stash<DelayedUpdate> _delayedUpdate;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<DelayedUpdate>();
            _delayedUpdate = World.GetStash<DelayedUpdate>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var delayedUpdate = ref _delayedUpdate.Get(entity);

                if (delayedUpdate.RemainingFrames <= 0)
                    _active.Add(entity);
            }
        }
    }
}
