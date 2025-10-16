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
    public sealed class Init_MoveTransformTargetEntity : ISystem
    {
        private Filter _filter;
        private Stash<InitialPosition> _initialPosition;
        private Stash<TargetEntity> _targetEntity;
        private Stash<TransformFromConfig> _transformFromConfig;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveTransform, Active, TargetEntity>()
                .Without<Initialized>().Build();
            _targetEntity = World.GetStash<TargetEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var targetEntity = ref _targetEntity.Get(entity);
                targetEntity.EntitySet = false;
            }
        }
    }
}