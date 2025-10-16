using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class StaticParticles : MonoBehaviour
    {
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false,DraggableItems = false)]
        [LabelText(" ")] 
        public StaticParticleData[] Entries;

        private Dictionary<string, StaticParticleData> _map;

        public void Initialize()
        {
            _map = new Dictionary<string, StaticParticleData>();

            foreach (var entry in Entries)
                _map[entry.Id] = entry;
        }

        public ParticleSystemEmitter WithId(string id) =>
            _map[id].ParticleSystem;
    }

    [Serializable]
    public class StaticParticleData
    {
        [HideLabel] [HorizontalGroup("0")]
        public string Id;
        [HideLabel] [HorizontalGroup("0")]
        public ParticleSystemEmitter ParticleSystem;
    }
}