using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Mob Config", fileName = "MobConfig")]
    public class NpcConfig : ScriptableObject
    {
        public int ID;
        public NpcProvider Prefab;
        public int Health;
        public int Score;
        
        [Space]
        public int MaxAmount;
        
        [Space]
        public int SpawnBatch;
        public float SpawnInterval;

        [Space]
        public string DeathParticleId;
        public Vector3 DeathParticleOffset;
        
        [Space]
        public bool SpawnOnStart;
    }
}