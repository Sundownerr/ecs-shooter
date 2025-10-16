using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct HaveTarget_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        [HorizontalGroup("HaveTargetConfig")]
        public TargetCheckType CheckType;

        [HorizontalGroup("HaveTargetConfig")]
        [HideLabel]
        [ShowIf(nameof(NeedsAmount))]
        public int Amount; // Only used when CheckType is AtLeastAmount

        private bool NeedsAmount() => CheckType == TargetCheckType.AtLeastAmount;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityHaveTarget.Set(entity, new AbilityHaveTarget
            {
                CheckType = CheckType,
                Amount = Amount
            });
        }
    }
}
