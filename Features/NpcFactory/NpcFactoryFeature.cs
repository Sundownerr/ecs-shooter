using Game.Systems;

namespace Game.Features
{
    public class NpcFactoryFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new CheckEnemySpawnRequestSystem());
            System(new NpcFactorySystem());

            System(new AddNpcComponentsSystem());
            // System(new AddEnemyToAliveEnemies());
            System(new AddNpcAbilitySystem());
            System(new AddNpcStateMachinesSystem());
        }
    }
}