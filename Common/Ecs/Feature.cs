using Scellecs.Morpeh;

namespace Game.Features
{
    public abstract class Feature
    {
        private SystemsGroup _group;
        

        public SystemsGroup AddTo(World world)
        {
            _group = world.CreateSystemsGroup();
            BuildGroup();

            return _group;
        }

        protected abstract void BuildGroup();

        protected void System(ISystem system) => _group.AddSystem(system);

        protected void Initializer(IInitializer initializer) => _group.AddInitializer(initializer);
    }
}