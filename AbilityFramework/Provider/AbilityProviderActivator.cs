using EcsMagic.CommonComponents;
using Game.Providers;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class AbilityProviderActivator : EntityProvider, IProviderActivator
    {
        public bool DestroyWhenWorldChanges = true;
        public bool ActivateAbility;
        public bool ActivateOnStart = true;
        public AbilityProvider Target;

        public void Awake()
        {
            if (!ActivateOnStart)
                return;

            ProviderActivatorManager.Register(this);
        }

        public void ActivateProvider()
        {
            var world = World.Default;

            // creating entity for user of target ability (this provider activator acts as user)
            Initialize(world);

            // creating entity for target ability
            // "Entity" is a provider entity
            var abilityEntity = Target.Create(Entity, world);

            var transformValue = gameObject.transform;
            StaticStash.ReferenceTransform.Set(Entity, new Reference<Transform> {Value = transformValue,});
            StaticStash.WorldPosition.Set(Entity, new WorldPosition {Value = transformValue.position,});
            StaticStash.ProviderActivator.Set(Entity, new ProviderActivator {
                ProviderEntity = Entity,
                ActivatedEntity = abilityEntity,
            });

            if (ActivateAbility)
                Target.Activate();

            if (DestroyWhenWorldChanges) {
                StaticStash.ReactOn_WorldChanged.Add(Entity);
                StaticStash.MarkToDestroyWhenWorldChanged.Add(Entity);
            }
        }
    }
}