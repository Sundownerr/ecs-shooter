using Scellecs.Morpeh;
using UnityEngine;

namespace SDW.EcsMagic.Triggers
{
    public class TriggerActivator : MonoBehaviour
    {
        public TriggerProvider TriggerProvider;

        private void Start()
        {
            TriggerProvider.InitializeTrigger(World.Default);
        }
    }
}