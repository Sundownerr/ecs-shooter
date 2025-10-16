using Game.Features;
using UnityEngine;

namespace Game
{
    public class FlyingAgentProvider : MonoBehaviour
    {
        public float MinimumHeight = 10f;
        public float GroundCheckRadius = 1f;
        public float UpForce = 10f;
        public float ForwardForce = 10f;
        public float StopDistance = 10f;
    }
}