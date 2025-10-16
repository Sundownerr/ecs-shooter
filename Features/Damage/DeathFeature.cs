using Game.Components;
using Game.Systems;

namespace Game.Features
{
    public class DeathFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new Remove<DiedNow>());
            System(new CheckIfHealthDepletedSystem());
        }
    }
}