using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [DefaultExecutionOrder(-9999)]
    public class SceneValidator : MonoBehaviour
    {
        public string ValidSceneName;

        
        private void Awake()
        {
            if (!EntryPoint.StartedFromEntryPoint) {
                SceneManager.LoadScene(ValidSceneName);
            }
        }
    }
}