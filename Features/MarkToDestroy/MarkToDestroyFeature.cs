using Game.Systems;

namespace Game.Features
{
    public class MarkToDestroyFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new MarkToDestryoWhenLevelChangedSystem());

            System(new MarkNpcWillBeDestroyedSystem());
            System(new MarkDeadDamagableDestroySystem());

            System(new MarkStateMachinesToDestroySystem());
            System(new MarkDestroyWeaponSystem());
            System(new MarkStateMachinePartsToDestroy());
            System(new MarkAbilitiesDestroySystem());
        }
    }
}