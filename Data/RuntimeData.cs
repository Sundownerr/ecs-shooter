using System;
using Game.Data;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace Game
{
    [Serializable]
    public class RuntimeData
    {
        public int CurrentLevelIndex;
        public PlayerProvider Player;
        public Camera Camera;
        public StaticParticles StaticParticles;
        public AsyncOperationHandle<SceneInstance> CurrentLevelHandle;
    }
}