using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Init_MoveTransformDurationSystem : ISystem
    {
        private Filter _filter;
        private Stash<MoveTransform> _moveTransform;
        private Stash<Duration> _duration;
      
        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveTransform, Active, Duration>().Without<Initialized>().Build();
         
            _moveTransform = World.GetStash<MoveTransform>();
            _duration = World.GetStash<Duration>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveTransform = ref _moveTransform.Get(entity);
                ref var duration = ref _duration.Get(entity);
                duration.Max = moveTransform.Config.Duration.Evaluate(Random.value);
            }
        }
    }
}
