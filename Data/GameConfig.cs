using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "SO/Game Config", fileName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [Header("Settings")]
        public int TargetFps;
        public int VSyncCount;
        public bool StartFromMenu;
        public LayerMask WeaponHitLayer;

        [Space]
        [Header("Score Settings")]
        [InlineProperty]
        [HideLabel]
        public ScoreSettings ScoreSettings;

        [Space]
        [Header("Spawner Landing Points")]
        [InlineProperty]
        [HideLabel]
        public SpawnerLandingPointSettings SpawnerLandingPoints;

        [Space]
        [Header("Scenes")]
        public AssetReference StartMenu;
        [ListDrawerSettings(ShowIndexLabels = false)]
        public LevelEntry[] Levels;

        [Space]
        [Header("Prefabs")]
        public GameObject PlayerPrefab;
        public GameObject StaticParticlesPrefab;
        
        [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false)]
        public List<PlayerWeaponEntry> PlayerWeapons;
    }
    
    
    [Serializable]
    public class PlayerWeaponEntry
    {
        [HorizontalGroup("0")]
        [HideLabel]
        public GameObject WeaponPrefab;
        [HorizontalGroup("0")]
        [HideLabel]
        public PlayerInputKey Key;
    }

    public enum PlayerInputKey
    {
        None = 0,
        Weapon1 = 1,
        Weapon2 = 2,
        Weapon3 = 3,
        Weapon4 = 4,
    }
   
}