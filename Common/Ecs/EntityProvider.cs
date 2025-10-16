using System;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Providers
{
    public class EntityProvider : MonoBehaviour
    {
        [NonSerialized]
        public Entity Entity;

        public Entity Initialize(World world) =>
            SetEntity(world.CreateEntity());

        public Entity SetEntity(Entity entity)
        {
            Entity = entity;
            return Entity;
        }
    }
}