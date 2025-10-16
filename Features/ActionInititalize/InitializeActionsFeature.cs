using Game.Systems;

namespace Game.Features
{
    public class InitializeActionsFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new Init_MoveTransformLerpFactorSystem());
            System(new Init_MoveTransformDurationSystem());
            System(new Init_MoveTransformInitialPosition());
            System(new Init_MoveTransformTargetEntity());
            System(new Init_RotateTransformTargetEntity());
            System(new Init_MoveTransformInitialTargetPositionSystem());
            System(new Init_MoveTransformDistanceToTargetSystem());
            System(new Init_MoveTransformDurationDistanceModifierSystem());

            System(new AddInitializedTagToActivePartsSystem());
            System(new RemoveInitializedTagOnDeactivatedPartsSystem());
        }
    }
}