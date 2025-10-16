using UnityEngine;

namespace Game
{
    public class HitscanShootingComponent : WeaponShootingComponent
    {
        public float MaxDistance = 100f;
        public float RayRadius = 0.5f;
        public Transform PlaceAtRayEnd;
    }
}