using System;
using Ability;
using Game;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct LineRenderer_AbilityAction : IAbilityAction
    {
      
        [HideLabel] public LineRenderer LineRenderer;
        [HorizontalGroup("O")]
        [HideLabel] public LineRendererConfig.LineRendererOptions Options;

        [HorizontalGroup("O")]
        [HideLabel]
        [ShowIf(nameof(IsSetPosition))]
        public int Index;

        [HorizontalGroup("P")]
        [HideLabel]
        [ShowIf(nameof(IsSetPosition))]
        public PositionConfig PositionConfig;

        public void AddTo(Entity entity)
        {
            StaticStash.LineRendererSetPosition.Set(entity, new LineRendererSetPosition
            {
                Config = new LineRendererConfig
                {
                    LineRenderer = LineRenderer,
                    Options = Options,
                    Index = Index,
                    PositionConfig = PositionConfig
                }
            });

            PositionConfig.AddTo(entity);
        }

        private bool IsSetPosition() => Options == LineRendererConfig.LineRendererOptions.SetPosition;
    }
}
