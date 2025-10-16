using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_SetGameObjectActiveSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<SetGameObjectActive> _setGameObjectActive;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<SetGameObjectActive, Active>();

            // Initialize stashes
            _setGameObjectActive = World.GetStash<SetGameObjectActive>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var setGameObjectActive = ref _setGameObjectActive.Get(entity);
                
                // Debug.Log( $" SetGameObjectActiveSystem {setGameObjectActive.Config.Target.name} {setGameObjectActive.Config.Active}");

                setGameObjectActive.Config.Target.SetActive(setGameObjectActive.Config.Active);

                _active.Remove(entity);
            }
        }
    }
}
