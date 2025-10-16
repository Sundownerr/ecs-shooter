using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct RigidbodyTargetConfig
    {
        [HideLabel] public TargetRigidbody TargetRigidbody;
        [HideLabel]
        [ShowIf(nameof(IsCustomTargetRigidbody))]
        public Rigidbody CustomTargetRigidbody;
        private bool IsCustomTargetRigidbody() => TargetRigidbody == TargetRigidbody.OtherRigidbody;
    }
}
