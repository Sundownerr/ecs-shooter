using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;

namespace Game.Systems
{
    public static class PlayerEcsUtility
    {
        private static Stash<WillBeDestroyed> _willBeDestroyed;
        private static Filter _playerFilter;

        public static void Initialize(World world)
        {
            _playerFilter = Entities.With<Player>();
            _willBeDestroyed = world.GetStash<WillBeDestroyed>();
        }

        public static void DestroyPlayer() =>
            _willBeDestroyed.Add(_playerFilter.First());
    }
}