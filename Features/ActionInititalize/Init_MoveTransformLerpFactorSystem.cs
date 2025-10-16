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
    public sealed class Init_MoveTransformLerpFactorSystem : ISystem
    {
        private Filter _filter;
        private Stash<MoveTransform> _moveTransform;
     
        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveTransform, Active>().Without<Initialized>().Build();
         
            _moveTransform = World.GetStash<MoveTransform>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveTransform = ref _moveTransform.Get(entity);
                moveTransform.Config.Duration.LerpFactor = Random.value;
                moveTransform.Config.XOffset.LerpFactor = Random.value;
                moveTransform.Config.YOffset.LerpFactor = Random.value;
                moveTransform.Config.ZOffset.LerpFactor = Random.value;
            }
        }
    }
}
