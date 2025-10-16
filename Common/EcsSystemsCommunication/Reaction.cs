using Scellecs.Morpeh;

namespace Game.Systems
{
    public static class Reaction
    {
        public static void TriggerReaction<T>(this Filter filter, Stash<T> stash) where T : struct, IComponent
        {
            foreach (var entity in filter)
                stash.Add(entity);
        }
    }
}