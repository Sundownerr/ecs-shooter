using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct MMFeedback_AbilityAction : IAbilityAction
    {
        [HideLabel]
        public MMF_Player Value;

        public void AddTo(Entity entity)
        {
            StaticStash.PlayMMFeedback.Set(entity, new PlayMMFeedback
            {
                Value = Value
            });
        }
    }
}
