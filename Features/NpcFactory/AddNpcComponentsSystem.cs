using System.Collections.Generic;
using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.WeaponComponents;
using ProjectDawn.Navigation.Hybrid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddNpcComponentsSystem : ISystem
    {
        private readonly List<WeaponProvider> _weapons = new();
        private Stash<Active> _active;
        private Stash<Reference<Animator>> _animator;
        private Stash<CanFly> _canFly;
        private Stash<CanWalk> _canWalk;
        private Stash<CustomTargetingTransform> _customTargetingTransform;
        private Stash<DamageApplied> _damageApplied;
        private Stash<DamageDealer> _damageDealer;
        private Stash<PlayParticleOnDeath> _deathParticle;
        private Stash<DelayedUpdate> _delayedUpdate;
        private Stash<DistanceToTarget> _distanceToTarget;
        private Stash<Event_NpcCreated> _eventNpcCreated;
        private Filter _filter;
        private Stash<FlyingAgent> _flyingAgent;
        private Stash<Health> _health;
        private Stash<MarkToDestroyWhenLevelChanged> _markToDestroyWhenWorldChanged;
        private Stash<Reference<AgentAuthoring>> _navMeshAgent;
        private Stash<Npc> _npc;
        private Stash<ReactOn_LevelChanged> _reactOnWorldChanged;
        private Stash<Request_CreateWeapon> _requestCreateWeapon;
        private Stash<ScoreReward> _scoreReward;
        private Stash<TargetWorldPosition> _targetWorldPosition;
        private Stash<Reference<Transform>> _transform;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_NpcCreated>();

            _eventNpcCreated = World.GetStash<Event_NpcCreated>();
            _npc = World.GetStash<Npc>();
            _deathParticle = World.GetStash<PlayParticleOnDeath>();
            _transform = World.GetStash<Reference<Transform>>();
            _damageApplied = World.GetStash<DamageApplied>();
            _delayedUpdate = World.GetStash<DelayedUpdate>();
            _worldPosition = World.GetStash<WorldPosition>();
            _distanceToTarget = World.GetStash<DistanceToTarget>();
            _targetWorldPosition = World.GetStash<TargetWorldPosition>();
            _customTargetingTransform = World.GetStash<CustomTargetingTransform>();
            _navMeshAgent = World.GetStash<Reference<AgentAuthoring>>();
            _canWalk = World.GetStash<CanWalk>();
            _flyingAgent = World.GetStash<FlyingAgent>();
            _canFly = World.GetStash<CanFly>();
            _animator = World.GetStash<Reference<Animator>>();
            _health = World.GetStash<Health>();
            _active = World.GetStash<Active>();
            _reactOnWorldChanged = World.GetStash<ReactOn_LevelChanged>();
            _markToDestroyWhenWorldChanged = World.GetStash<MarkToDestroyWhenLevelChanged>();
            _requestCreateWeapon = World.GetStash<Request_CreateWeapon>();
            _damageDealer = World.GetStash<DamageDealer>();
            _scoreReward = World.GetStash<ScoreReward>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var evt = ref _eventNpcCreated.Get(entity);

                ref var npcComponent = ref _npc.Add(evt.Entity);
                npcComponent.Instance = evt.Instance;
                npcComponent.Config = evt.Config;

                _active.Add(evt.Entity);

                if (!string.IsNullOrEmpty(evt.Config.DeathParticleId))
                    _deathParticle.Add(evt.Entity);

                _transform.Set(evt.Entity, new Reference<Transform> {Value = evt.Instance.transform,});

                _reactOnWorldChanged.Add(evt.Entity);
                _markToDestroyWhenWorldChanged.Add(evt.Entity);

                _damageApplied.Add(evt.Entity);
                _delayedUpdate.Add(evt.Entity);
                _worldPosition.Add(evt.Entity);
                _distanceToTarget.Add(evt.Entity);
                _targetWorldPosition.Add(evt.Entity);
                _damageDealer.Set(evt.Entity, new DamageDealer {Type = DamageDealerType.Enemy,});
                _health.Set(evt.Entity, new Health {Value = evt.Config.Health,});
                _scoreReward.Set(evt.Entity, new ScoreReward {Value = evt.Config.Score,});

                if (evt.Instance.TargetingTransform != null)
                    _customTargetingTransform.Set(evt.Entity,
                        new CustomTargetingTransform {Value = evt.Instance.TargetingTransform,});

                if (evt.Instance.CanWalk()) {
                    _navMeshAgent.Set(evt.Entity, new Reference<AgentAuthoring> {
                        Value = evt.Instance.GetComponent<AgentAuthoring>(),
                    });

                    _canWalk.Add(evt.Entity);
                }

                if (evt.Instance.CanFly()) {
                    _flyingAgent.Set(evt.Entity, new FlyingAgent {
                        Instance = evt.Instance.GetComponent<FlyingAgentProvider>(),
                        Rigidbody = evt.Instance.GetComponent<Rigidbody>(),
                    });

                    _canFly.Add(evt.Entity);
                }

                if (evt.Instance.Animator != null)
                    _animator.Set(evt.Entity, new Reference<Animator> {Value = evt.Instance.Animator,});

                _weapons.Clear();
                evt.Instance.GetComponentsInChildren(false, _weapons);

                foreach (var weapon in _weapons) {
                    _requestCreateWeapon.CreateRequest(new Request_CreateWeapon {
                        Instance = weapon,
                        WeaponUserEntity = evt.Entity,
                    });
                }
            }
        }
    }
}