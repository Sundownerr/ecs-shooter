using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game
{
    public  class AddressablesService
    {
        public  AsyncOperationHandle<SceneInstance> LoadSceneAsync(AssetReference scene) =>
            Addressables.LoadSceneAsync(scene);
    }
}