using Game;
using Scellecs.Morpeh;

namespace Ability.Targeting
{
    public struct UserTarget_AbilityTarget : IAbilityTarget
    {
        public void AddTo(Entity entity) =>
            StaticStash.UserTarget.Add(entity);
    }
}