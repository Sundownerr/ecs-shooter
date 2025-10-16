using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitializeEcsExtentionsSystem : IInitializer
    {
        public void Dispose() { }

        public void OnAwake()
        {
            StaticStash.Initialize(World);

            Entities.Initialize(World);
            Request.Initialize(World);
            Event.Initialize(World);
            LevelsEcsUtility.Initialize(World);
            PlayerEcsUtility.Initialize(World);

            ProviderActivatorManager.ClearOnReload();

            Debug.Log("- ECS Extensions Initialized");
        }

        public World World { get; set; }
    }
}