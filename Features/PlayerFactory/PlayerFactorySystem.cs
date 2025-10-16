using System.Collections.Generic;
using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.AbilityComponents;
using Game.Components;
using Game.Data;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerFactorySystem : ISystem
    {
        private readonly RuntimeData _runtimeData;
        private readonly GameConfig _gameConfig;
        private Stash<AbilitiesList> _abilitiesList;
        private Stash<AccumulatedMana> _accumulatedMana;
        private Filter _currentLevelFilter;
        private Stash<CustomTargetingTransform> _customTargetingTransform;
        private Stash<DamageApplied> _damageApplied;
        private Stash<DamageDealer> _damageDealer;
        private Stash<DamageDirection> _damageDirection;
        private Stash<Event_PlayerCreated> _eventPlayerCreated;
        private Filter _filter;
        private Stash<FloatStats> _floatStats;
        private Stash<Gravity> _gravity;
        private Stash<Health> _health;
        private Stash<IsGrounded> _isGrounded;
        private Stash<IsGroundedSettings> _isGroundedSettings;
        private Stash<LastNavMeshPosition> _lastNavMeshPosition;
        private Stash<Level> _level;
        private Stash<LookDirection> _lookDirection;
        private Stash<MaxHealth> _maxHealth;
        private Stash<MoveDirection> _moveDirection;
        private Stash<Player> _player;
        private Stash<PlayerConfig> _playerConfig;
        private Stash<PlayerInput_ChangeWorldBackward_WasPressed> _playerInput_ChangeWorldBackward_WasPressed;
        private Stash<PlayerInput_ChangeWorldForward_WasPressed> _playerInput_ChangeWorldForward_WasPressed;
        private Stash<PlayerInput_Dash_WasPressed> _playerInput_Dash_WasPressed;
        private Stash<PlayerInput_Jump_IsPressed> _playerInput_Jump_IsPressed;
        private Stash<PlayerInput_LookDirection> _playerInput_LookDirection;
        private Stash<PlayerInput_MoveBackward_IsPressed> _playerInput_MoveBackward_IsPressed;
        private Stash<PlayerInput_MoveDirection> _playerInput_MoveDirection;
        private Stash<PlayerInput_MoveForward_IsPressed> _playerInput_MoveForward_IsPressed;
        private Stash<PlayerInput_MoveLeft_IsPressed> _playerInput_MoveLeft_IsPressed;
        private Stash<PlayerInput_MoveRight_IsPressed> _playerInput_MoveRight_IsPressed;
        private Stash<PlayerInput_PrimaryAbility_IsPressed> _playerInput_PrimaryAbility_IsPressed;
        private Stash<PlayerInput_PrimaryAbility_WasPressed> _playerInput_PrimaryAbility_WasPressed;
        private Stash<PlayerInput_PrimaryAbility_WasReleased> _playerInput_PrimaryAbility_WasReleased;
        private Stash<PlayerInput_PrimaryAttack_IsPressed> _playerInput_PrimaryAttack_IsPressed;
        private Stash<PlayerInput_PrimaryAttack_WasPressed> _playerInput_PrimaryAttack_WasPressed;
        private Stash<PlayerInput_PrimaryAttack_WasReleased> _playerInput_PrimaryAttack_WasReleased;
        private Stash<PlayerInput_SecondaryAbility_IsPressed> _playerInput_SecondaryAbility_IsPressed;
        private Stash<PlayerInput_SecondaryAbility_WasPressed> _playerInput_SecondaryAbility_WasPressed;
        private Stash<PlayerInput_SecondaryAbility_WasReleased> _playerInput_SecondaryAbility_WasReleased;
        private Stash<PlayerInput_SecondaryAttack_IsPressed> _playerInput_SecondaryAttack_IsPressed;
        private Stash<PlayerInput_SecondaryAttack_WasPressed> _playerInput_SecondaryAttack_WasPressed;
        private Stash<PlayerInput_SecondaryAttack_WasReleased> _playerInput_SecondaryAttack_WasReleased;
        private Stash<PlayerInput_Sprint_IsPressed> _playerInput_Sprint_IsPressed;
        private Stash<PlayerInput_Sprint_WasPressed> _playerInput_Sprint_WasPressed;
        private Stash<PlayerInput_Sprint_WasReleased> _playerInput_Sprint_WasReleased;
        private Stash<PlayerInput_Weapon1_WasPressed> _weapon1_WasPressed;
        private Stash<PlayerInput_Weapon2_WasPressed> _weapon2_WasPressed;
        private Stash<PlayerInput_Weapon3_WasPressed> _weapon3_WasPressed;
        private Stash<PlayerInput_Weapon4_WasPressed> _weapon4_WasPressed;
        
        private Stash<ReactOn_LevelChanged> _reactOn_WorldChanged;
        private Stash<Reference<Rigidbody>> _referenceRigidbody;
        private Stash<Reference<Transform>> _referenceTransform;

        private Stash<Request_CreatePlayer> _requestCreatePlayer;
        private Stash<StateMachinesList> _stateMachineList;
        private Stash<VerticalVelocity> _verticalVelocity;
        private Stash<WorldPosition> _worldPosition;
        private Stash<WeaponChangeTimer> _weaponChangeTimer;
        private Stash<CanChangeWeapons> _canChangeWeapons;

        public PlayerFactorySystem(DataLocator dataLocator)
        {
            _gameConfig = dataLocator.Get<GameConfig>();
            _runtimeData = dataLocator.Get<RuntimeData>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_CreatePlayer>();
            _currentLevelFilter = Entities.With<CurrentLevel>();

            _requestCreatePlayer = World.GetStash<Request_CreatePlayer>();
            _eventPlayerCreated = World.GetStash<Event_PlayerCreated>();
            _level = World.GetStash<Level>();
            _floatStats = World.GetStash<FloatStats>();
            _accumulatedMana = World.GetStash<AccumulatedMana>();
            _customTargetingTransform = World.GetStash<CustomTargetingTransform>();
            _worldPosition = World.GetStash<WorldPosition>();
            _player = World.GetStash<Player>();
            _referenceTransform = World.GetStash<Reference<Transform>>();
            _referenceRigidbody = World.GetStash<Reference<Rigidbody>>();
            _playerConfig = World.GetStash<PlayerConfig>();
            _gravity = World.GetStash<Gravity>();
            _isGroundedSettings = World.GetStash<IsGroundedSettings>();
            _health = World.GetStash<Health>();
            _maxHealth = World.GetStash<MaxHealth>();
            _damageApplied = World.GetStash<DamageApplied>();
            _lookDirection = World.GetStash<LookDirection>();
            _moveDirection = World.GetStash<MoveDirection>();
            _verticalVelocity = World.GetStash<VerticalVelocity>();
            _damageDirection = World.GetStash<DamageDirection>();
            _isGrounded = World.GetStash<IsGrounded>();
            _lastNavMeshPosition = World.GetStash<LastNavMeshPosition>();
            _playerInput_LookDirection = World.GetStash<PlayerInput_LookDirection>();
            _playerInput_MoveDirection = World.GetStash<PlayerInput_MoveDirection>();
            _playerInput_MoveForward_IsPressed = World.GetStash<PlayerInput_MoveForward_IsPressed>();
            _playerInput_MoveBackward_IsPressed = World.GetStash<PlayerInput_MoveBackward_IsPressed>();
            _playerInput_MoveLeft_IsPressed = World.GetStash<PlayerInput_MoveLeft_IsPressed>();
            _playerInput_MoveRight_IsPressed = World.GetStash<PlayerInput_MoveRight_IsPressed>();
            _playerInput_Jump_IsPressed = World.GetStash<PlayerInput_Jump_IsPressed>();
            _playerInput_Sprint_IsPressed = World.GetStash<PlayerInput_Sprint_IsPressed>();
            _playerInput_Sprint_WasPressed = World.GetStash<PlayerInput_Sprint_WasPressed>();
            _playerInput_Sprint_WasReleased = World.GetStash<PlayerInput_Sprint_WasReleased>();
            _playerInput_ChangeWorldForward_WasPressed = World.GetStash<PlayerInput_ChangeWorldForward_WasPressed>();
            _playerInput_ChangeWorldBackward_WasPressed = World.GetStash<PlayerInput_ChangeWorldBackward_WasPressed>();
            _playerInput_PrimaryAttack_IsPressed = World.GetStash<PlayerInput_PrimaryAttack_IsPressed>();
            _playerInput_PrimaryAttack_WasPressed = World.GetStash<PlayerInput_PrimaryAttack_WasPressed>();
            _playerInput_PrimaryAttack_WasReleased = World.GetStash<PlayerInput_PrimaryAttack_WasReleased>();
            _playerInput_SecondaryAttack_IsPressed = World.GetStash<PlayerInput_SecondaryAttack_IsPressed>();
            _playerInput_SecondaryAttack_WasPressed = World.GetStash<PlayerInput_SecondaryAttack_WasPressed>();
            _playerInput_SecondaryAttack_WasReleased = World.GetStash<PlayerInput_SecondaryAttack_WasReleased>();
            _playerInput_PrimaryAbility_WasPressed = World.GetStash<PlayerInput_PrimaryAbility_WasPressed>();
            _playerInput_PrimaryAbility_IsPressed = World.GetStash<PlayerInput_PrimaryAbility_IsPressed>();
            _playerInput_PrimaryAbility_WasReleased = World.GetStash<PlayerInput_PrimaryAbility_WasReleased>();
            _playerInput_SecondaryAbility_IsPressed = World.GetStash<PlayerInput_SecondaryAbility_IsPressed>();
            _playerInput_SecondaryAbility_WasPressed = World.GetStash<PlayerInput_SecondaryAbility_WasPressed>();
            _playerInput_SecondaryAbility_WasReleased = World.GetStash<PlayerInput_SecondaryAbility_WasReleased>();
            _playerInput_Dash_WasPressed = World.GetStash<PlayerInput_Dash_WasPressed>();
            _reactOn_WorldChanged = World.GetStash<ReactOn_LevelChanged>();
            _abilitiesList = World.GetStash<AbilitiesList>();
            _stateMachineList = World.GetStash<StateMachinesList>();
            _damageDealer = World.GetStash<DamageDealer>();
            _weapon1_WasPressed = World.GetStash<PlayerInput_Weapon1_WasPressed>();
            _weapon2_WasPressed = World.GetStash<PlayerInput_Weapon2_WasPressed>();
            _weapon3_WasPressed = World.GetStash<PlayerInput_Weapon3_WasPressed>();
            _weapon4_WasPressed = World.GetStash<PlayerInput_Weapon4_WasPressed>();
            _weaponChangeTimer = World.GetStash<WeaponChangeTimer>();
            _canChangeWeapons = World.GetStash<CanChangeWeapons>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var currentLevel = ref _level.Get(_currentLevelFilter.First());
                var spawnPoint = currentLevel.Instance.PlayerSpawnPoint;
                var position = spawnPoint.position;
                var rotation = spawnPoint.rotation;

                var playerInstance = Object.Instantiate(_gameConfig.PlayerPrefab, position, rotation)
                    .GetComponent<PlayerProvider>();

                _runtimeData.Player = playerInstance;

                var playerEntity = playerInstance.Initialize(World);
                _player.Set(playerEntity, new Player {
                    Instance = playerInstance, 
                    WeaponsOnKeys =  new Dictionary<PlayerInputKey, WeaponProvider>()
                });

                var abilityList = new List<Entity>();
                foreach (var abilityProvider in playerInstance.Abilities)
                    abilityList.Add(abilityProvider.Create(playerEntity, World));
                _abilitiesList.Set(playerEntity, new AbilitiesList {List = abilityList,});

                var stateMachineList = new List<Entity>();
                foreach (var stateMachine in playerInstance.StateMachines)
                    stateMachineList.Add(stateMachine.Create(playerEntity, World));
                _stateMachineList.Set(playerEntity, new StateMachinesList {List = stateMachineList,});

                Stats<float> stats = new Stats<float>()
                    .AddNew(Stat.MoveSpeed, playerInstance.PlayerConfig.MoveSpeed)
                    .AddNew(Stat.Mana, playerInstance.MaxMana)
                    .AddNew(Stat.MaxMana, playerInstance.MaxMana)
                    .AddNew(Stat.ManaRegen, playerInstance.ManaRegen);

                _floatStats.Set(playerEntity, new FloatStats {Value = stats,});
                _accumulatedMana.Set(playerEntity, new AccumulatedMana {Value = 0f,});
                _customTargetingTransform.Set(playerEntity,
                    new CustomTargetingTransform {Value = playerInstance.TargetingTransform,});
                _worldPosition.Set(playerEntity, new WorldPosition {Value = playerInstance.transform.position,});
               
                _referenceTransform.Set(playerEntity, new Reference<Transform> {Value = playerInstance.transform,});
                _referenceRigidbody.Set(playerEntity, new Reference<Rigidbody> {Value = playerInstance.Rigidbody,});
                _playerConfig.Set(playerEntity, playerInstance.PlayerConfig);
                _gravity.Set(playerEntity, playerInstance.Gravity);
                _isGroundedSettings.Set(playerEntity, playerInstance.IsGroundedSettings);
                _health.Set(playerEntity, new Health {Value = playerInstance.Health,});
                _maxHealth.Set(playerEntity, new MaxHealth {Value = playerInstance.Health,});
                _damageApplied.Add(playerEntity);
                _damageDealer.Set(playerEntity, new DamageDealer {Type = DamageDealerType.Player,});
                _weaponChangeTimer.Add(playerEntity);
                _canChangeWeapons.Add(playerEntity);

                _lookDirection.Add(playerEntity);
                _moveDirection.Add(playerEntity);
                _verticalVelocity.Add(playerEntity);
                _damageDirection.Add(playerEntity);
                _isGrounded.Add(playerEntity);
                _lastNavMeshPosition.Add(playerEntity);

                _playerInput_LookDirection.Add(playerEntity);
                _playerInput_MoveDirection.Add(playerEntity);
                _playerInput_MoveForward_IsPressed.Add(playerEntity);
                _playerInput_MoveBackward_IsPressed.Add(playerEntity);
                _playerInput_MoveLeft_IsPressed.Add(playerEntity);
                _playerInput_MoveRight_IsPressed.Add(playerEntity);
                _playerInput_Jump_IsPressed.Add(playerEntity);
                _playerInput_Sprint_IsPressed.Add(playerEntity);
                _playerInput_Sprint_WasPressed.Add(playerEntity);
                _playerInput_Sprint_WasReleased.Add(playerEntity);
                _playerInput_ChangeWorldForward_WasPressed.Add(playerEntity);
                _playerInput_ChangeWorldBackward_WasPressed.Add(playerEntity);
                _playerInput_PrimaryAttack_IsPressed.Add(playerEntity);
                _playerInput_PrimaryAttack_WasPressed.Add(playerEntity);
                _playerInput_PrimaryAttack_WasReleased.Add(playerEntity);
                _playerInput_SecondaryAttack_IsPressed.Add(playerEntity);
                _playerInput_SecondaryAttack_WasPressed.Add(playerEntity);
                _playerInput_SecondaryAttack_WasReleased.Add(playerEntity);
                _playerInput_PrimaryAbility_WasPressed.Add(playerEntity);
                _playerInput_PrimaryAbility_IsPressed.Add(playerEntity);
                _playerInput_PrimaryAbility_WasReleased.Add(playerEntity);
                _playerInput_SecondaryAbility_IsPressed.Add(playerEntity);
                _playerInput_SecondaryAbility_WasPressed.Add(playerEntity);
                _playerInput_SecondaryAbility_WasReleased.Add(playerEntity);
                _playerInput_Dash_WasPressed.Add(playerEntity);
                _weapon1_WasPressed.Add(playerEntity);
                _weapon2_WasPressed.Add(playerEntity);
                _weapon3_WasPressed.Add(playerEntity);
                _weapon4_WasPressed.Add(playerEntity);

                _reactOn_WorldChanged.Add(playerEntity);

                _eventPlayerCreated.CreateEvent();

                entity.CompleteRequest();
            }
        }
    }
}