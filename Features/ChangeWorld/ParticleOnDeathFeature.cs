using Game.Data;

namespace Game.Features
{
    public class ParticleOnDeathFeature : Feature
    {
        private readonly DataLocator _dataLocator;

        public ParticleOnDeathFeature(DataLocator dataLocator)
        {
            _dataLocator = dataLocator;
        }
        
        protected override void BuildGroup()
        {
            System(new PlayParticleOnNpcDeathFeature(_dataLocator));
        }
    }
}