using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{

    // [Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public struct PooledEntity : IComponent { }
    //
    // public static class EntityPool
    // {
    //     private static List<Entity> _pool ;
    //     private static List<Entity> _returned;
    //     private static Stash<PooledEntity> _pooledEntityStash;
    //     private static World _world;
    //
    //     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    //     private static void ClearOnRestart()
    //     {
    //         _world = null;
    //         _pool.Clear();
    //         _returned.Clear();
    //         _pooledEntityStash = null;
    //     }
    //
    //     public static void Initialize(World world)
    //     {
    //         _world = world;
    //         _pool = new List<Entity>();
    //         _returned = new List<Entity>();
    //         _pooledEntityStash = world.GetStash<PooledEntity>();
    //     }
    //
    //     public static void Return(Entity entity) =>
    //         _returned.Add(entity);
    //
    //     public static void Update()
    //     {
    //         for(var i = _returned.Count - 1; i >= 0; i--) {
    //             _pool.Add(_returned[i]);
    //             _returned.RemoveAt(i);
    //         }
    //     }
    //
    //     public static Entity Get()
    //     {
    //         var poolCount = _pool.Count;
    //
    //         if (poolCount > 0) {
    //             var entity = _pool[poolCount - 1];
    //             _pool.RemoveAt(poolCount - 1);
    //
    //             return entity;
    //         }
    //
    //         var newEntity = _world.CreateEntity();
    //         _pooledEntityStash.Add(newEntity);
    //
    //         return newEntity;
    //     }
    // }
}