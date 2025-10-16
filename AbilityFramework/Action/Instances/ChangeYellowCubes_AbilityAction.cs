using System;
using Ability;
using Game;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct ChangeYellowCubes_AbilityAction : IAbilityAction
    {
        public int Delta;
        public bool AsGatheredOnLevel;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityChangeYellowCubes.Set(entity, new AbilityChangeYellowCubes
            {
                Delta = Delta,
                AsGatheredOnLevel = AsGatheredOnLevel
            });
        }
    }
}
