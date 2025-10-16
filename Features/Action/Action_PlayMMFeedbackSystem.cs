using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_PlayMMFeedbackSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<PlayMMFeedback> _playMMFeedback;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<PlayMMFeedback, Active>();

            // Initialize stashes
            _playMMFeedback = World.GetStash<PlayMMFeedback>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var playMMFeedback = ref _playMMFeedback.Get(entity);
                var mmFeedback = playMMFeedback.Value;

                mmFeedback.PlayFeedbacks();

                _active.Remove(entity);
            }
        }
    }
}
