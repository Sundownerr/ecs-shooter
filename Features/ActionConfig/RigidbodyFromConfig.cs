using System;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Components
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct RigidbodyFromConfig : IComponent
    {
        public RigidbodyTargetConfig Config;
        public Rigidbody Rigidbody;
    }
}
