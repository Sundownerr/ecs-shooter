using Game.Systems;

namespace Game.Features
{
    public class ConditionsFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new Check_DistanceToTargetLessThanSystem());
            System(new Check_DistanceToTargetGreaterThanSystem());
            System(new Check_NotOnCooldownSystem());
            System(new Check_AbilityActivatedSystem());
            System(new Check_AbilityHaveTargetSystem());
            System(new Check_TriggerEnterSystem());
            System(new Check_TriggerStaySystem());
            System(new Check_TriggerExitSystem());
            System(new Check_WeaponTriggerPulledSystem());
            System(new Check_WeaponTriggerReleasedSystem());

            System(new Check_CheckSphereSystem());
            System(new Check_CheckRaycastHitSystem());

            System(new Check_PrimaryAbilityPressedSystem());
            System(new Check_SecondaryAbilityPressedSystem());
            System(new Check_SprintPressedSystem());
            System(new Check_PrimaryAttackPressedSystem());
            System(new Check_SecondaryAttackPressedSystem());
            System(new Check_DashPressedSystem());

            System(new Check_PrimaryAbilityWasPressedSystem());
            System(new Check_SecondaryAbilityWasPressedSystem());
            System(new Check_SprintWasPressedSystem());
            System(new Check_PrimaryAttackWasPressedSystem());
            System(new Check_SecondaryAttackWasPressedSystem());
            System(new Check_DashWasPressedSystem());

            System(new Check_PrimaryAbilityReleasedSystem());
            System(new Check_SecondaryAbilityReleasedSystem());
            System(new Check_SprintReleasedSystem());
            System(new Check_PrimaryAttackReleasedSystem());
            System(new Check_SecondaryAttackReleasedSystem());

            System(new Check_HasManaSystem());
            System(new Check_HasBeenDamagedSystem());
            System(new Check_VelocityGreaterThanSystem());
            System(new Check_VelocityLowerThanSystem());
            System(new Check_GameObjectActiveSystem());
            System(new Check_GatheredYellowCubesSystem());
            System(new Check_TotalYellowCubesSystem());
            System(new Check_HealthSystem());

            System(new AddFullfilledForwardConditionsSystem());
            System(new AddFullfilledCancelConditionsSystem());

            System(new CheckCancelConditionsMetSystem());
            System(new DeactivateCompletedForwardConditionsSystem());
        }
    }
}
