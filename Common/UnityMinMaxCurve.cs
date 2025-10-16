using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [Serializable]
    public struct UnityMinMaxCurve
    {
        [LabelWidth(10)]
        public ParticleSystem.MinMaxCurve Value;
        [NonSerialized]
        public float LerpFactor;
        public float Evaluate(float time) => Value.Evaluate(time, LerpFactor);

        public float EvaluateRandom() => Value.Evaluate(Random.value, LerpFactor);
    }
}