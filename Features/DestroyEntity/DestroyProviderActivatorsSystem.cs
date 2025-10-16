using EcsMagic.CommonComponents;
using Game.Components;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyProviderActivatorsSystem : ISystem
    {
        private Filter _filter;
        private Stash<ProviderActivator> _providerActivator;
        private Stash<Reference<Transform>> _transform;
        private Stash<WillBeDestroyed> _willbeDestroyed;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ProviderActivator, WillBeDestroyed>();
            _providerActivator = World.GetStash<ProviderActivator>();
            _transform = World.GetStash<Reference<Transform>>();
            _willbeDestroyed = World.GetStash<WillBeDestroyed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var activator = ref _providerActivator.Get(entity);
                ref Reference<Transform> transform = ref _transform.Get(entity);

                if (!_willbeDestroyed.Has(activator.ActivatedEntity))
                    _willbeDestroyed.Add(activator.ActivatedEntity);
                
                // Debug.Log($"Destroying {transform.Value.gameObject.name}");

                Object.Destroy(transform.Value.gameObject);
                World.RemoveEntity(activator.ProviderEntity);
            }
        }
    }
}