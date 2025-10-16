using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public static class Entities
    {
        private static World _world;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearOnReload() =>
            _world = null;

        public static void Initialize(World world) =>
            _world = world;

        public static Filter With<T>() where T : struct, IComponent =>
            _world.Filter.With<T>().Build();

        public static Filter With<T1, T2>() where T1 : struct, IComponent
                                            where T2 : struct, IComponent =>
            _world.Filter.With<T1>().With<T2>().Build();

        public static Filter With<T1, T2, T3>() where T1 : struct, IComponent
                                                where T2 : struct, IComponent
                                                where T3 : struct, IComponent =>
            _world.Filter.With<T1>().With<T2>().With<T3>().Build();

        public static Filter With<T1, T2, T3, T4>() where T1 : struct, IComponent
                                                    where T2 : struct, IComponent
                                                    where T3 : struct, IComponent
                                                    where T4 : struct, IComponent =>
            _world.Filter.With<T1>().With<T2>().With<T3>().With<T4>().Build();

        public static ref T Singleton<T>() where T : struct, IComponent =>
             ref _world.GetStash<T>().Get(_world.Filter.With<T>().Build().First());
    }
}