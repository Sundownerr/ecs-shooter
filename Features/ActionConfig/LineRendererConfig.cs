using System;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct LineRendererConfig
    {
        public enum LineRendererOptions { SetPosition = 0, }

        [HideLabel] public LineRenderer LineRenderer;
        [HorizontalGroup("O")]
        [HideLabel] public LineRendererOptions Options;

        [HorizontalGroup("O")]
        [HideLabel] [ShowIf(nameof(IsSetPosition))]
        public int Index;

        [HorizontalGroup("P")]
        [HideLabel] [ShowIf(nameof(IsSetPosition))]
        public PositionConfig PositionConfig;

        private bool IsSetPosition() => Options == LineRendererOptions.SetPosition;

        public void AddTo(Entity entity)
        {
            ref var lineRendererAction = ref entity.AddComponent<LineRendererSetPosition>();
            lineRendererAction.Config = this;

            PositionConfig.AddTo(entity);
        }
    }
}