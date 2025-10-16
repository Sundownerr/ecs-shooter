using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Providers;
using UnityEngine;

namespace Game
{
    public class PlayerProvider : EntityProvider
    {
        public int Health;
        public int MaxMana;
        public int ManaRegen;
        [Space]
        public PlayerConfig PlayerConfig;
        public Gravity Gravity;
        public IsGroundedSettings IsGroundedSettings;


        [Space]
        public Rigidbody Rigidbody;
        public Transform CinemachineCameraTarget;
        public Transform WeaponParent;

        [Space]
        public Transform TargetingTransform;
        
        [Space]
        public AbilityProvider[] Abilities;
        public StateMachineProvider[] StateMachines;
    }
}
