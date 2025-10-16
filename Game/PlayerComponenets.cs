using System;
using System.Collections.Generic;
using Game;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace EcsMagic.PlayerComponenets
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PlayerDashAbility : IComponent
    {
        public Entity Player;
        public float Force;
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PlayerConfig : IComponent
    {
        public float MoveSpeed;
        public float JumpHeight;
        public float LookRotationSpeed;
        public float FlyingAscendSpeed;
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Player : IComponent
    {
        public PlayerProvider Instance;
        public bool Jumped;
        public Dictionary<PlayerInputKey, WeaponProvider> WeaponsOnKeys;
        public WeaponProvider ActiveWeapon;
    }
}