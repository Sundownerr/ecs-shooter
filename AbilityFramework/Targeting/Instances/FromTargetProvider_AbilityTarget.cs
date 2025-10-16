using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability.Targeting
{
    [Serializable]
    public struct FromTargetProvider_AbilityTarget : IAbilityTarget
    {
        [HideLabel]
        public TargetProvider TargetProvider;

        public void AddTo(Entity entity) =>
            StaticStash.TargetsFromTargetProvider.Set(entity, new TargetsFromTargetProvider {
                Config = new FromTargetProviderConfig {
                    TargetProvider = TargetProvider,
                },
            });
    }
}