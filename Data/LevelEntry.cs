using System;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace Game.Data
{
    [Serializable]
    public class LevelEntry
    {
        [HideLabel]
        [HorizontalGroup("0")]
        public AssetReference Scene;

        [HideLabel]
        [HorizontalGroup("0")]

#if UNITY_EDITOR
        [InlineButton(nameof(UpdateName))]
#endif
        public string Name;

#if UNITY_EDITOR
        private void UpdateName() => Name = Scene.editorAsset.name;
#endif
    }
}