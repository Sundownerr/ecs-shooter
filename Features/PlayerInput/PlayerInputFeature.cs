using Game.Systems;

namespace Game.Features
{
    public class PlayerInputFeature : Feature
    {
        private readonly FPSInput _input;

        public PlayerInputFeature(FPSInput input)
        {
            _input = input;
        }

        protected override void BuildGroup()
        {
            System(new UpdatePlayerInputSystem(_input));
            System(new UpdatePlayerMoveDirectionSystem());
           
            System(new PlayerLookDirectionSystem());
            System(new PlayerPositionSystem());
        }
    }
}