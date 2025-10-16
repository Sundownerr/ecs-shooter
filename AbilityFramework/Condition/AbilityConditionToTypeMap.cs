using System;
using System.Collections.Generic;
using Ability.Identifications;
using Ability.Utilities;

namespace Ability
{
    public static class AbilityConditionToTypeMap
    {
        public static List<(int, Dictionary<int, Type>)> Create() => new() {
            ConditionId.Activated.Group()
                .With<AbilityActivated_AbilityCondition>(ConditionId.Activated),

            ConditionId.DistanceToTarget.Group()
                .With<DistanceToTargetGreaterThan_AbilityCondition>(ConditionId.GreaterThan)
                .With<DistanceToTargetLessThan_AbilityCondition>(ConditionId.LessThan),

            ConditionId.Velocity.Group()
                .With<VelocityGreaterThan_AbilityCondition>(ConditionId.GreaterThan)
                .With<VelocityLowerThan_AbilityCondition>(ConditionId.LessThan),

            ConditionId.InputPressed.Group()
                .With<PrimaryAbilityPressed_AbilityCondition>(ConditionId.PrimaryAbility)
                .With<SecondaryAbilityPressed_AbilityCondition>(ConditionId.SecondaryAbility)
                .With<SprintPressed_AbilityCondition>(ConditionId.Sprint)
                .With<PrimaryAttackPressed_AbilityCondition>(ConditionId.PrimaryAttack)
                .With<SecondaryAttackPressed_AbilityCondition>(ConditionId.SecondaryAttack)
                .With<DashPressed_AbilityCondition>(ConditionId.Dash),

            ConditionId.InputReleased.Group()
                .With<PrimaryAbilityReleased_AbilityCondition>(ConditionId.PrimaryAbility)
                .With<SecondaryAbilityReleased_AbilityCondition>(ConditionId.SecondaryAbility)
                .With<SprintReleased_AbilityCondition>(ConditionId.Sprint)
                .With<PrimaryAttackReleased_AbilityCondition>(ConditionId.PrimaryAttack)
                .With<SecondaryAttackReleased_AbilityCondition>(ConditionId.SecondaryAttack),

            ConditionId.InputWasPressed.Group()
                .With<PrimaryAbilityWasPressed_AbilityCondition>(ConditionId.PrimaryAbility)
                .With<SecondaryAbilityWasPressed_AbilityCondition>(ConditionId.SecondaryAbility)
                .With<SprintWasPressed_AbilityCondition>(ConditionId.Sprint)
                .With<PrimaryAttackWasPressed_AbilityCondition>(ConditionId.PrimaryAttack)
                .With<SecondaryAttackWasPressed_AbilityCondition>(ConditionId.SecondaryAttack)
                .With<DashWasPressed_AbilityCondition>(ConditionId.Dash),

            ConditionId.Physics.Group()
                .With<CheckSphere_AbilityCondition>(ConditionId.CheckSphere)
                .With<CheckRaycastHit_AbilityCondition>(ConditionId.CheckRaycastHit)
                .With<Trigger_AbilityCondition>(ConditionId.Trigger),

            ConditionId.Stats.Group()
                .With<Mana_AbilityCondition>(ConditionId.Mana)
                .With<Health_AbilityCondition>(ConditionId.Health),

            ConditionId.WeaponTrigger.Group()
                .With<WeaponTrigger_AbilityCondition>(ConditionId.WeaponTrigger),

            ConditionId.HasBeenDamaged.Group()
                .With<HasBeenDamaged_AbilityCondition>(ConditionId.HasBeenDamaged),

            ConditionId.HaveTarget.Group()
                .With<HaveTarget_AbilityCondition>(ConditionId.HaveTarget),

            ConditionId.GameObjectActive.Group()
                .With<GameObjectActive_AbilityCondition>(ConditionId.GameObjectActive),

            ConditionId.YellowCubes.Group()
                .With<YellowCubes_AbilityCondition>(ConditionId.GatheredOnLevel)
                .With<TotalYellowCubes_AbilityCondition>(ConditionId.Total),
            
            ConditionId.NotOnCooldown.Group()
                .With<NotOnCooldown_AbilityCondition>(ConditionId.NotOnCooldown),
        };
    }
}