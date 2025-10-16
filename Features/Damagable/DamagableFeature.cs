using Game.Systems;

namespace Game.Features
{
    public class DamagableFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new InitializeDamagableSystem());
            // System(new CheckDamagableHealthPercentSystem());
         
        }
    }
}