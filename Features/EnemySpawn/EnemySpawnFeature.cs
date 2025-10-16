using Game.Systems;

namespace Game.Features
{
    public class EnemySpawnFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new CreateEnemySpawnersOnLevelSystem());
            System(new CreateEnemyOnTimerCompletedSystem());
        }
    }
}