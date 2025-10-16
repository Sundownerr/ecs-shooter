using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    public enum TargetGameObject
    {
        UserGameObject = 0,
        TargetGameObject = 1,
        CustomGameObject = 2,
    }

    [Serializable]
    public struct GameObjectConfig
    {
        [HideLabel] public TargetGameObject TargetGameObject;
        [HideLabel]
        [ShowIf(nameof(IsCustomTargetGameObject))]
        public GameObject CustomTargetGameObject;
        private bool IsCustomTargetGameObject() => TargetGameObject == TargetGameObject.CustomGameObject;
    }
}
