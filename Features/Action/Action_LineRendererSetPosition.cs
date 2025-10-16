using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_LineRendererSetPosition : ISystem
    {
        private Filter _filter;
        private Stash<LineRendererSetPosition> _lineRendererSetPosition;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<LineRendererSetPosition, Active>();

            _lineRendererSetPosition = World.GetStash<LineRendererSetPosition>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var lineRendererAction = ref _lineRendererSetPosition.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);

                lineRendererAction.Config.LineRenderer.SetPosition(lineRendererAction.Config.Index,
                    positionFromConfig.Position);

                // Debug.Log($"LineRenderer Position {lineRendererAction.Config.Index}: {positionFromConfig.Position}");
                _active.Remove(entity);
            }
        }
    }
}
