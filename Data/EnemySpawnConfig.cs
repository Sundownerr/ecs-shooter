using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "SO/Enemy Spawn Config", fileName = "EnemySpawnConfig", order = 0)]
    public class EnemySpawnConfig : ScriptableObject
    {
        public NpcConfig NpcConfig;

        [LabelText(" ")]
        [TableList(AlwaysExpanded = true)]
        public List<EnemySpawnConfigEntry> Entries;
    }

    [Serializable]
    public struct EnemySpawnConfigEntry
    {
        [InlineProperty]
        public PercentTime PercentTime;
        
        [MinValue(0)]
        [TableColumnWidth(120, false)]
        public float NextSpawnDelay;
        
        [MinValue(0)]
        [TableColumnWidth(120, false)]
        public float SpawnRadius;
        
        [MinValue(0)]
        [TableColumnWidth(80, false)]
        public float Amount;
    }

    [Serializable]
    public struct PercentTime
    {
        [Range(0f, 1f)]
        [TableColumnWidth(160)]
        [LabelText("%")]
        [LabelWidth(20)]
        public float Percent;

        [MinValue(0)]
        [LabelText("at")]
        [LabelWidth(20)]
        [TableColumnWidth(70, false)]
        [SuffixLabel("sec", true)]
        public float Time;
    }
}