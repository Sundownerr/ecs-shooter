using System;
using System.Collections.Generic;
using Game.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Flags]
    public enum NpcMovementType { Walking = 1 << 0, Flying = 1 << 1, }

    public class NpcProvider : EntityProvider
    {
        public NpcMovementType MovementType;
        public Transform Model;
        public Transform TargetingTransform;
        public Animator Animator;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        public List<StateMachineProvider> StateMachines;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        public List<AbilityProvider> Abilities;

        public bool CanWalk() => (MovementType & NpcMovementType.Walking) != 0;
        public bool CanFly() => (MovementType & NpcMovementType.Flying) != 0;
    }
}