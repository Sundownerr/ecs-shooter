using System;
using System.Collections.Generic;
using Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class SpawnerLandingPointSettings
    {
        public float UpdateInterval = 0.5f;
        public int MaxActivePoints;
        public bool RandomizePoints;

        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false)]
        [LabelText("Activate spawner point when (all)", SdfIconType.SunFill)]
        public List<SpawnerConstraintWrapper> ActivationConstraints;

        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false)]
        [LabelText("Deactivate spawner point when (any)", SdfIconType.Sun)]
        public List<SpawnerConstraintWrapper> DeactivationConstraints;
    }
}