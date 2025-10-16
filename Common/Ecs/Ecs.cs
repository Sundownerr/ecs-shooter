using Game.Features;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class Ecs : MonoBehaviour
    {
        private static World _world;
        private readonly EcsFeatures _features;

        public void Update() =>
            _world.Update(Time.deltaTime);

        public void FixedUpdate() =>
            _world.FixedUpdate(Time.fixedDeltaTime);

        public void LateUpdate()
        {
            var dt = Time.deltaTime;
            _world.LateUpdate(dt);
            _world.CleanupUpdate(dt);
        }

        public void Initialize(EcsFeatures features)
        {
            _world = World.Default.IsNullOrDisposed() ? World.Create() : World.Default;
            _world.UpdateByUnity = false;
          
            features.AddTo(_world);
            enabled = true;

            Debug.Log($"- ECS Initialized {_world}");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void ClearOnReload()
        {
            if (!_world.IsNullOrDisposed()) {
                Debug.Log(
                    $"- Created world disposed: {_world.metrics.entities} entities, {_world.metrics.filters} filters");
                _world.Dispose();
                _world = null;
            }
        
            if (!World.Default.IsNullOrDisposed()) {
                World.Default.Dispose();
                Debug.Log("- Default world disposed");
            }
        
            Debug.Log("- ECS Disposed");
        }
    }
}