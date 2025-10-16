using System;
using Game;
using Scellecs.Morpeh;

namespace Ability
{
    [Serializable]
    public struct ChangeWorldForward_AbilityAction : IAbilityAction
    {
        public void AddTo(Entity entity) =>
            StaticStash.CreateRequest_ChangeWorldForward.Add(entity);
    }
}