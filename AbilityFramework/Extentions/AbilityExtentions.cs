using Scellecs.Morpeh;

namespace Game
{
    public static class AbilityExtentions
    {
        public static void ActivateAbility(this Entity abilityEntity)
        {
            ref var activated = ref StaticStash.AbilityActivatedFromScript.Get(abilityEntity);
            activated.Value = true;

            if (StaticStash.TargetingParts.Has(abilityEntity)) {
                ref var targetingParts = ref StaticStash.TargetingParts.Get(abilityEntity);

                foreach (var entity in targetingParts.List) {
                    if (!StaticStash.Active.Has(entity))
                        StaticStash.Active.Add(entity);
                }
            }
        }

        public static void DeactivateAbility(this Entity abilityEntity)
        {
            ref var activated = ref StaticStash.AbilityActivatedFromScript.Get(abilityEntity);
            activated.Value = false;

            if (StaticStash.TargetingParts.Has(abilityEntity)) {
                ref var targetingParts = ref StaticStash.TargetingParts.Get(abilityEntity);

                foreach (var entity in targetingParts.List)
                    StaticStash.Active.Remove(entity);
            }
        }

        public static void SetAbilityActive(this Entity abilityEntity, bool active)
        {
            if (active)
                abilityEntity.ActivateAbility();
            else
                abilityEntity.DeactivateAbility();
        }
    }
}