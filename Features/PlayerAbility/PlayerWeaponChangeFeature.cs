using Game.Systems;

namespace Game.Features
{
    public class PlayerWeaponChangeFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new PlayerWeaponChangeSystem());
        }
    }
}