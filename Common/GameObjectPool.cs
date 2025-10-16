using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class GameObjectPool
    {
        private const int MaxGameobjectPerTransform = 256;
        private static readonly string PoolName = $"GameObject Pool ({MaxGameobjectPerTransform})";

        private static readonly Dictionary<GameObject, Queue<GameObject>> _pools = new();
        private static readonly Dictionary<GameObject, GameObject> _prefabLookup = new();
        private static int _instanciatedObjects;
        private static int _poolIndex;

        private static readonly List<Transform> _poolTransforms = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetPool()
        {
            _pools.Clear();
            _prefabLookup.Clear();
            _poolTransforms.Clear();
            _instanciatedObjects = 0;
            _poolIndex = 0;
        }

        public static void ClearPool()
        {
            foreach (Queue<GameObject> pool in _pools.Values) {
                while (pool.Count > 0) {
                    var instance = pool.Dequeue();

                    if (instance != null)
                        Object.Destroy(instance);
                }
            }

            foreach (var poolTransform in _poolTransforms) {
                if (poolTransform != null)
                    Object.Destroy(poolTransform.gameObject);
            }

            ResetPool();
        }

        public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(prefab, out Queue<GameObject> pool)) {
                pool = new Queue<GameObject>();
                _pools[prefab] = pool;
            }

            GameObject instance;

            if (pool.Count > 0) {
                instance = pool.Dequeue();
                instance.transform.SetPositionAndRotation(position, rotation);

                // Debug.Log($"<-[-] Reusing {prefab.name}");
            }
            else {
                instance = Object.Instantiate(prefab, position, rotation, NextParent());
                _prefabLookup[instance] = prefab;
                _instanciatedObjects++;

                // Debug.Log($"<-[+] Created new {prefab.name} ");
            }

            instance.SetActive(true);
            return instance;
        }

        public static void CreateFirstPool() =>
            _poolTransforms.Add(new GameObject(PoolName).transform);

        public static Transform NextParent()
        {
            if (_instanciatedObjects <= MaxGameobjectPerTransform)
                return _poolTransforms[_poolIndex];

            _poolTransforms.Add(new GameObject(PoolName).transform);
            _instanciatedObjects = 0;
            _poolIndex++;

            return _poolTransforms[_poolIndex];
        }

        public static void Return(GameObject instance)
        {
            if (instance == null) return;

            if (!_prefabLookup.TryGetValue(instance, out var prefab)) {
                Debug.LogWarning($"Trying to return an object that wasn't created by the pool: {instance.name}");
                return;
            }

            // Debug.Log($"-> Returning  {prefab.name}");

            if (!_pools.TryGetValue(prefab, out Queue<GameObject> pool)) {
                pool = new Queue<GameObject>();
                _pools[prefab] = pool;
            }

            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }
}