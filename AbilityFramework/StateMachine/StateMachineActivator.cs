using EcsMagic.CommonComponents;
using Game.Providers;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class StateMachineActivator : EntityProvider, IProviderActivator
    {
        public bool DestroyWhenWorldChanges = true;
        [Space]
        public StateMachineProvider Provider;

        private void Awake() =>
            ProviderActivatorManager.Register(this);

        public void ActivateProvider()
        {
            var world = World.Default;

            Initialize(world);
            var stateMachineEntity = Provider.Create(Entity, world);

            StaticStash.ReferenceTransform.Set(Entity, new Reference<Transform> {Value = transform,});
            StaticStash.WorldPosition.Set(Entity, new WorldPosition {Value = transform.position,});
            StaticStash.ProviderActivator.Set(Entity, new ProviderActivator {
                ProviderEntity = Entity,
                ActivatedEntity =  stateMachineEntity
            });


            if (DestroyWhenWorldChanges) {
                StaticStash.ReactOn_WorldChanged.Add(Entity);
                StaticStash.MarkToDestroyWhenWorldChanged.Add(Entity);
            }
        }
    }
}