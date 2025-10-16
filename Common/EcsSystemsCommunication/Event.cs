using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    public static class Event
    {
        private static Stash<OneFrame> _oneFrame;
        private static World _world;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearOnReload() =>
            _world = null;

        public static void Initialize(World world)
        {
            _world = world;
            _oneFrame = world.GetStash<OneFrame>();
        }

        public static void CreateEvent<T>(this Stash<T> stash, in T component) where T : struct, IComponent
        {
            var entity = _world.CreateEntity();
            stash.Set(entity, component);
            _oneFrame.Add(entity);
        }

        public static void CreateEvent<T>(this Stash<T> stash) where T : struct, IComponent
        {
            var entity = _world.CreateEntity();
            stash.Add(entity);
            _oneFrame.Add(entity);
        }
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct OneFrame : IComponent { }
}