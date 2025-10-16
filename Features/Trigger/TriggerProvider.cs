using System;
using System.Collections.Generic;
using Game.Providers;
using Scellecs.Morpeh;
using UnityEngine;

namespace SDW.EcsMagic.Triggers
{
    public class TriggerProvider : EntityProvider
    {
        public bool LogEnter;
        [NonSerialized] public readonly HashSet<Entity> EnterSubscribers = new();
        [NonSerialized] public readonly HashSet<Entity> ExitSubscribers = new();
        [NonSerialized] public readonly HashSet<Entity> StaySubscribers = new();
        private Stash<NotifyTriggerEnter> _notifyTriggerEnter;
        private Stash<NotifyTriggerExit> _notifyTriggerExit;
        private Stash<NotifyTriggerStay> _notifyTriggerStay;
        [NonSerialized] public List<GameObject> Enter = new();
        [NonSerialized] public List<GameObject> Exit = new();
        [NonSerialized] public List<GameObject> Stay = new();

        private void OnTriggerEnter(Collider other)
        {
            Enter.Add(other.gameObject);

            foreach (var enterSubscriber in EnterSubscribers) {
                if (!enterSubscriber.Has<NotifyTriggerEnter>())
                    _notifyTriggerEnter.Add(enterSubscriber);
            }

            if (LogEnter)
                Debug.Log($"{gameObject.name} Trigger Enter: {other.gameObject.name}");
        }

        private void OnTriggerExit(Collider other)
        {
            Exit.Add(other.gameObject);
            Enter.Remove(other.gameObject);
            Stay.Remove(other.gameObject);

            foreach (var exitSubscriber in ExitSubscribers) {
                if (!exitSubscriber.Has<NotifyTriggerExit>())
                    _notifyTriggerExit.Add(exitSubscriber);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Stay.Add(other.gameObject);

            foreach (var staySubscriber in StaySubscribers) {
                if (!staySubscriber.Has<NotifyTriggerStay>())
                    _notifyTriggerStay.Add(staySubscriber);
            }
        }

        public void InitializeTrigger(World world)
        {
            Initialize(world);
            ref var trigger = ref Entity.AddComponent<Trigger>();
            trigger.Instance = this;

            Entity.AddComponent<WorldTriggerTag>();

            _notifyTriggerEnter = world.GetStash<NotifyTriggerEnter>();
            _notifyTriggerExit = world.GetStash<NotifyTriggerExit>();
            _notifyTriggerStay = world.GetStash<NotifyTriggerStay>();
        }
    }
}