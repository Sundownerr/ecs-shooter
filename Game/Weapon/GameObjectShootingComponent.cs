using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class GameObjectShootingComponent : WeaponShootingComponent
    {
        public Transform ShootPoint;
        public GameObject ProjectilePrefab;
        public float Radius;
        [LabelWidth(20)] public AnimationCurve X;
        [LabelWidth(20)] public AnimationCurve Y;
        [LabelWidth(20)] public AnimationCurve Z;
        [LabelWidth(30)] public AnimationCurve Speed;
    }
}