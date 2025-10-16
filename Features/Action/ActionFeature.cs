using Game.Systems;

namespace Game.Features
{
    public class ActionFeature : Feature
    {
        private readonly DataLocator _dataLocator;
        private readonly ServiceLocator _serviceLocator;

        public ActionFeature(DataLocator dataLocator, ServiceLocator serviceLocator)
        {
            _dataLocator = dataLocator;
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup()
        {
            System(new Action_CreateGameObjectSystem());
            System(new Action_CreateAbilityProviderSystem());
            System(new Action_CreateStateMachineProviderSystem());
            System(new Action_SetGameObjectActiveSystem());

            System(new Action_PlayMMFeedbackSystem());

            System(new Action_EnableUserNavMeshAgentSystem());
            System(new Action_DisableUserNavMeshAgentSystem());
            System(new Action_ChangeUserNavMeshAgentSpeedSystem());

            System(new Action_SetAbilityActiveSystem());
            System(new Action_PlayParticle());
            System(new Action_ChangeAnimatorSettingsSystem());
            System(new Action_DamageSystem(_serviceLocator));

            System(new Action_RotateTransformSystem());
            System(new Action_MoveTransformSystem());

            System(new Action_WeaponTriggerSystem());

            System(new Action_RigidbodyActionAddForceSystem());
            System(new Action_RigidbodyActionSetKinematicSystem());

            System(new Action_ChangeYellowCubesSystem());
            System(new Action_CreateEnemySystem());
            System(new Action_LineRendererSetPosition());
            System(new Action_ChangeManaSystem());
            System(new Action_RegenerateFullHealthSystem());
            System(new Action_RegenerateFullManaSystem());
            System(new Action_DashSystem());
            System(new Action_SprintSystem());
            System(new Action_ChangeFloatStatValueSystem());
            System(new Action_AddFloatStatModifierSystem());
            System(new Action_RemoveFloatStatModifierSystem());
            System(new Action_PlayStaticParticleSystem(_dataLocator));
            System(new Action_ReturnToPoolSystem());
            System(new Action_DestroyGameObjectSystem());
            System(new Action_ChangeComponentEnabledSystem());
            System(new Action_Create_Rq_ChangeWorldForwardSystem());
        }
    }
}