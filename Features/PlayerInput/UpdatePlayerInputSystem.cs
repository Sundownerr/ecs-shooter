using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerInputSystem : ISystem
    {
        private readonly FPSInput _input;
        private Filter _changeWorldBackward_WasPressedFilter;
        private Stash<PlayerInput_ChangeWorldBackward_WasPressed> _changeWorldBackwardWasPressed;
        private Filter _changeWorldForward_WasPressedFilter;
        private Stash<PlayerInput_ChangeWorldForward_WasPressed> _changeWorldForwardWasPressed;
        private Stash<PlayerInput_Dash_WasPressed> _dashWasPressed;
        private Filter _dashWasPressedFilter;
        private Filter _look_DirectionFilter;
        private Stash<PlayerInput_LookDirection> _lookDirection;
        private Filter _move_DirectionFilter;
        private Filter _moveBackward_PressedFilter;
        private Stash<PlayerInput_MoveBackward_IsPressed> _moveBackwardIsPressed;
        private Stash<PlayerInput_MoveDirection> _moveDirection;
        private Filter _moveForward_PressedFilter;
        private Stash<PlayerInput_MoveForward_IsPressed> _moveForwardIsPressed;
        private Filter _moveLeft_PressedFilter;
        private Stash<PlayerInput_MoveLeft_IsPressed> _moveLeftIsPressed;
        private Filter _moveRight_PressedFilter;
        private Stash<PlayerInput_MoveRight_IsPressed> _moveRightIsPressed;
        private Filter _primaryAbility_PressedFilter;
        private Filter _primaryAbility_WasPressedFilter;

        private Filter _primaryAbility_WasReleasedFilter;
        private Stash<PlayerInput_PrimaryAbility_IsPressed> _primaryAbilityIsPressed;
        private Stash<PlayerInput_PrimaryAbility_WasPressed> _primaryAbilityWasPressed;
        private Stash<PlayerInput_PrimaryAbility_WasReleased> _primaryAbilityWasReleased;
        private Filter _primaryAttack_PressedFilter;
        private Filter _primaryAttack_WasPressedFilter;
        private Filter _primaryAttack_WasReleasedFilter;
        private Stash<PlayerInput_PrimaryAttack_IsPressed> _primaryAttackIsPressed;
        private Stash<PlayerInput_PrimaryAttack_WasPressed> _primaryAttackWasPressed;
        private Stash<PlayerInput_PrimaryAttack_WasReleased> _primaryAttackWasReleased;
        private Filter _secondaryAbility_PressedFilter;

        private Filter _secondaryAbility_WasPressedFilter;
        private Filter _secondaryAbility_WasReleasedFilter;
        private Stash<PlayerInput_SecondaryAbility_IsPressed> _secondaryAbilityIsPressed;
        private Stash<PlayerInput_SecondaryAbility_WasPressed> _secondaryAbilityWasPressed;
        private Stash<PlayerInput_SecondaryAbility_WasReleased> _secondaryAbilityWasReleased;
        private Filter _secondaryAttack_PressedFilter;
        private Filter _secondaryAttack_WasPressedFilter;
        private Filter _secondaryAttack_WasReleasedFilter;
        private Stash<PlayerInput_SecondaryAttack_IsPressed> _secondaryAttackIsPressed;
        private Stash<PlayerInput_SecondaryAttack_WasPressed> _secondaryAttackWasPressed;
        private Stash<PlayerInput_SecondaryAttack_WasReleased> _secondaryAttackWasReleased;
        private Stash<PlayerInput_SlowMo_WasPressed> _slowMoWasPressed;
        private Filter _slowTime_WasPressedFilter;
        private Filter _space_PressedFilter;
        private Stash<PlayerInput_Jump_IsPressed> _spaceIsPressed;
        private Filter _sprint_PressedFilter;
        private Filter _sprint_WasPressedFilter;
        private Filter _sprint_WasReleasedFilter;
        private Stash<PlayerInput_Sprint_IsPressed> _sprintIsPressed;
        private Stash<PlayerInput_Sprint_WasPressed> _sprintWasPressed;
        private Stash<PlayerInput_Sprint_WasReleased> _sprintWasReleased;
        private Filter _weapon1_wasPressedFilter;
        private Filter _weapon2_wasPressedFilter;
        private Filter _weapon3_wasPressedFilter;
        private Filter _weapon4_wasPressedFilter;
        private Stash<PlayerInput_Weapon1_WasPressed> _weapon1WasPressed;
        private Stash<PlayerInput_Weapon2_WasPressed> _weapon2WasPressed;
        private Stash<PlayerInput_Weapon3_WasPressed> _weapon3WasPressed;
        private Stash<PlayerInput_Weapon4_WasPressed> _weapon4WasPressed;

        public UpdatePlayerInputSystem(FPSInput input)
        {
            _input = input;
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _primaryAttack_PressedFilter = Entities.With<PlayerInput_PrimaryAttack_IsPressed>();
            _secondaryAttack_PressedFilter = Entities.With<PlayerInput_SecondaryAttack_IsPressed>();
            _secondaryAbility_PressedFilter = Entities.With<PlayerInput_SecondaryAbility_IsPressed>();
            _primaryAbility_PressedFilter = Entities.With<PlayerInput_PrimaryAbility_IsPressed>();
            _primaryAbility_WasPressedFilter = Entities.With<PlayerInput_PrimaryAbility_WasPressed>();
            _slowTime_WasPressedFilter = Entities.With<PlayerInput_SlowMo_WasPressed>();
            _sprint_WasPressedFilter = Entities.With<PlayerInput_Sprint_WasPressed>();
            _changeWorldForward_WasPressedFilter = Entities.With<PlayerInput_ChangeWorldForward_WasPressed>();
            _changeWorldBackward_WasPressedFilter = Entities.With<PlayerInput_ChangeWorldBackward_WasPressed>();
            _moveForward_PressedFilter = Entities.With<PlayerInput_MoveForward_IsPressed>();
            _moveBackward_PressedFilter = Entities.With<PlayerInput_MoveBackward_IsPressed>();
            _moveLeft_PressedFilter = Entities.With<PlayerInput_MoveLeft_IsPressed>();
            _moveRight_PressedFilter = Entities.With<PlayerInput_MoveRight_IsPressed>();
            _space_PressedFilter = Entities.With<PlayerInput_Jump_IsPressed>();
            _sprint_PressedFilter = Entities.With<PlayerInput_Sprint_IsPressed>();
            _move_DirectionFilter = Entities.With<PlayerInput_MoveDirection>();
            _look_DirectionFilter = Entities.With<PlayerInput_LookDirection>();
            _dashWasPressedFilter = Entities.With<PlayerInput_Dash_WasPressed>();
            _primaryAbility_WasReleasedFilter = Entities.With<PlayerInput_PrimaryAbility_WasReleased>();
            _secondaryAbility_WasReleasedFilter = Entities.With<PlayerInput_SecondaryAbility_WasReleased>();
            _sprint_WasReleasedFilter = Entities.With<PlayerInput_Sprint_WasReleased>();
            _primaryAttack_WasReleasedFilter = Entities.With<PlayerInput_PrimaryAttack_WasReleased>();
            _secondaryAttack_WasReleasedFilter = Entities.With<PlayerInput_SecondaryAttack_WasReleased>();
            _secondaryAbility_WasPressedFilter = Entities.With<PlayerInput_SecondaryAbility_WasPressed>();
            _primaryAttack_WasPressedFilter = Entities.With<PlayerInput_PrimaryAttack_WasPressed>();
            _secondaryAttack_WasPressedFilter = Entities.With<PlayerInput_SecondaryAttack_WasPressed>();
            _weapon1_wasPressedFilter = Entities.With<PlayerInput_Weapon1_WasPressed>();
            _weapon2_wasPressedFilter = Entities.With<PlayerInput_Weapon2_WasPressed>();
            _weapon3_wasPressedFilter = Entities.With<PlayerInput_Weapon3_WasPressed>();
            _weapon4_wasPressedFilter = Entities.With<PlayerInput_Weapon4_WasPressed>();
            
            _primaryAttackIsPressed = World.GetStash<PlayerInput_PrimaryAttack_IsPressed>();
            _secondaryAttackIsPressed = World.GetStash<PlayerInput_SecondaryAttack_IsPressed>();
            _secondaryAbilityIsPressed = World.GetStash<PlayerInput_SecondaryAbility_IsPressed>();
            _primaryAbilityIsPressed = World.GetStash<PlayerInput_PrimaryAbility_IsPressed>();
            _primaryAbilityWasPressed = World.GetStash<PlayerInput_PrimaryAbility_WasPressed>();
            _slowMoWasPressed = World.GetStash<PlayerInput_SlowMo_WasPressed>();
            _sprintWasPressed = World.GetStash<PlayerInput_Sprint_WasPressed>();
            _changeWorldForwardWasPressed = World.GetStash<PlayerInput_ChangeWorldForward_WasPressed>();
            _changeWorldBackwardWasPressed = World.GetStash<PlayerInput_ChangeWorldBackward_WasPressed>();
            _moveForwardIsPressed = World.GetStash<PlayerInput_MoveForward_IsPressed>();
            _moveBackwardIsPressed = World.GetStash<PlayerInput_MoveBackward_IsPressed>();
            _moveLeftIsPressed = World.GetStash<PlayerInput_MoveLeft_IsPressed>();
            _moveRightIsPressed = World.GetStash<PlayerInput_MoveRight_IsPressed>();
            _spaceIsPressed = World.GetStash<PlayerInput_Jump_IsPressed>();
            _sprintIsPressed = World.GetStash<PlayerInput_Sprint_IsPressed>();
            _dashWasPressed = World.GetStash<PlayerInput_Dash_WasPressed>();
            _primaryAbilityWasReleased = World.GetStash<PlayerInput_PrimaryAbility_WasReleased>();
            _secondaryAbilityWasReleased = World.GetStash<PlayerInput_SecondaryAbility_WasReleased>();
            _sprintWasReleased = World.GetStash<PlayerInput_Sprint_WasReleased>();
            _primaryAttackWasReleased = World.GetStash<PlayerInput_PrimaryAttack_WasReleased>();
            _secondaryAttackWasReleased = World.GetStash<PlayerInput_SecondaryAttack_WasReleased>();
            _secondaryAbilityWasPressed = World.GetStash<PlayerInput_SecondaryAbility_WasPressed>();
            _primaryAttackWasPressed = World.GetStash<PlayerInput_PrimaryAttack_WasPressed>();
            _secondaryAttackWasPressed = World.GetStash<PlayerInput_SecondaryAttack_WasPressed>();
            _moveDirection = World.GetStash<PlayerInput_MoveDirection>();
            _lookDirection = World.GetStash<PlayerInput_LookDirection>();
            _weapon1WasPressed = World.GetStash<PlayerInput_Weapon1_WasPressed>();
            _weapon2WasPressed = World.GetStash<PlayerInput_Weapon2_WasPressed>();
            _weapon3WasPressed = World.GetStash<PlayerInput_Weapon3_WasPressed>();
            _weapon4WasPressed = World.GetStash<PlayerInput_Weapon4_WasPressed>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var playerInput = _input.Player;

            var playerInputPrimaryAttack = playerInput.PrimaryAttack;
            var playerInputSecondaryAttack = playerInput.SecondaryAttack;
            var moveDirectionValue = playerInput.Move.ReadValue<Vector2>();
            var playerInputSecondaryAbility = playerInput.SecondaryAbility;
            var playerInputPrimaryAbility = playerInput.PrimaryAbility;
            var playerInputSprint = playerInput.Sprint;
            var lookDirectionValue = playerInput.Look.ReadValue<Vector2>();

            foreach (var entity in _primaryAttack_PressedFilter) {
                ref var primaryAttack = ref _primaryAttackIsPressed.Get(entity);
                primaryAttack.Value = playerInputPrimaryAttack.IsPressed();
            }

            foreach (var entity in _secondaryAttack_PressedFilter) {
                ref var secondaryAttack = ref _secondaryAttackIsPressed.Get(entity);
                secondaryAttack.Value = playerInputSecondaryAttack.IsPressed();
            }

            foreach (var entity in _moveForward_PressedFilter) {
                ref var moveForward = ref _moveForwardIsPressed.Get(entity);
                moveForward.Value = moveDirectionValue.y >= 1f;
            }

            foreach (var entity in _moveBackward_PressedFilter) {
                ref var moveBackward = ref _moveBackwardIsPressed.Get(entity);
                moveBackward.Value = moveDirectionValue.y <= -1f;
            }

            foreach (var entity in _moveLeft_PressedFilter) {
                ref var moveLeft = ref _moveLeftIsPressed.Get(entity);
                moveLeft.Value = moveDirectionValue.x <= -1f;
            }

            foreach (var entity in _moveRight_PressedFilter) {
                ref var moveRight = ref _moveRightIsPressed.Get(entity);
                moveRight.Value = moveDirectionValue.x >= 1f;
            }

            foreach (var entity in _secondaryAbility_PressedFilter) {
                ref var secondaryAbility = ref _secondaryAbilityIsPressed.Get(entity);
                secondaryAbility.Value = playerInputSecondaryAbility.IsPressed();
            }

            foreach (var entity in _primaryAbility_PressedFilter) {
                ref var primaryAbility = ref _primaryAbilityIsPressed.Get(entity);
                primaryAbility.Value = playerInputPrimaryAbility.IsPressed();
            }

            foreach (var entity in _space_PressedFilter) {
                ref var spacePressed = ref _spaceIsPressed.Get(entity);
                spacePressed.Value = playerInput.Jump.IsPressed();
            }

            foreach (var entity in _sprint_PressedFilter) {
                ref var sprintPressed = ref _sprintIsPressed.Get(entity);
                sprintPressed.Value = playerInputSprint.IsPressed();
            }

            foreach (var entity in _sprint_WasPressedFilter) {
                ref var sprintPressed = ref _sprintWasPressed.Get(entity);
                sprintPressed.Value = playerInputSprint.WasPerformedThisFrame();
            }

            foreach (var entity in _move_DirectionFilter) {
                ref var moveDirection = ref _moveDirection.Get(entity);
                moveDirection.Value = moveDirectionValue;
            }

            foreach (var entity in _look_DirectionFilter) {
                ref var lookDirection = ref _lookDirection.Get(entity);
                lookDirection.Value = lookDirectionValue;
            }

            foreach (var entity in _slowTime_WasPressedFilter) {
                ref var slowTimeWasPressed = ref _slowMoWasPressed.Get(entity);
                slowTimeWasPressed.Value = playerInput.SlowMoAbility.WasPerformedThisFrame();
            }

            foreach (var entity in _primaryAbility_WasPressedFilter) {
                ref var primaryAbilityWasPressed = ref _primaryAbilityWasPressed.Get(entity);
                primaryAbilityWasPressed.Value = playerInputPrimaryAbility.WasPerformedThisFrame();
            }

            foreach (var entity in _changeWorldForward_WasPressedFilter) {
                ref var changeWorldForwardWasPressed = ref _changeWorldForwardWasPressed.Get(entity);
                changeWorldForwardWasPressed.Value = playerInput.ChangeWorldForward.WasPerformedThisFrame();
            }

            foreach (var entity in _changeWorldBackward_WasPressedFilter) {
                ref var changeWorldBackwardWasPressed = ref _changeWorldBackwardWasPressed.Get(entity);
                changeWorldBackwardWasPressed.Value = playerInput.ChangeWorldBackward.WasPerformedThisFrame();
            }

            foreach (var entity in _dashWasPressedFilter) {
                ref var dashWasPressed = ref _dashWasPressed.Get(entity);
                dashWasPressed.Value = playerInput.Dash.WasPerformedThisFrame();
            }

            foreach (var entity in _primaryAbility_WasReleasedFilter) {
                ref var primaryAbilityWasReleased = ref _primaryAbilityWasReleased.Get(entity);
                primaryAbilityWasReleased.Value = playerInputPrimaryAbility.WasReleasedThisFrame();
            }

            foreach (var entity in _secondaryAbility_WasReleasedFilter) {
                ref var secondaryAbilityWasReleased = ref _secondaryAbilityWasReleased.Get(entity);
                secondaryAbilityWasReleased.Value = playerInputSecondaryAbility.WasReleasedThisFrame();
            }

            foreach (var entity in _sprint_WasReleasedFilter) {
                ref var sprintWasReleased = ref _sprintWasReleased.Get(entity);
                sprintWasReleased.Value = playerInputSprint.WasReleasedThisFrame();
            }

            foreach (var entity in _primaryAttack_WasReleasedFilter) {
                ref var primaryAttackWasReleased = ref _primaryAttackWasReleased.Get(entity);
                primaryAttackWasReleased.Value = playerInputPrimaryAttack.WasReleasedThisFrame();
            }

            foreach (var entity in _secondaryAttack_WasReleasedFilter) {
                ref var secondaryAttackWasReleased = ref _secondaryAttackWasReleased.Get(entity);
                secondaryAttackWasReleased.Value = playerInputSecondaryAttack.WasReleasedThisFrame();
            }

            foreach (var entity in _secondaryAbility_WasPressedFilter) {
                ref var secondaryAbilityWasPressed = ref _secondaryAbilityWasPressed.Get(entity);
                secondaryAbilityWasPressed.Value = playerInputSecondaryAbility.WasPerformedThisFrame();
            }

            foreach (var entity in _primaryAttack_WasPressedFilter) {
                ref var primaryAttackWasPressed = ref _primaryAttackWasPressed.Get(entity);
                primaryAttackWasPressed.Value = playerInputPrimaryAttack.WasPerformedThisFrame();
            }

            foreach (var entity in _secondaryAttack_WasPressedFilter) {
                ref var secondaryAttackWasPressed = ref _secondaryAttackWasPressed.Get(entity);
                secondaryAttackWasPressed.Value = playerInputSecondaryAttack.WasPerformedThisFrame();
            }

            foreach (var entity in _weapon1_wasPressedFilter) {
                ref var weapon1WasPressed = ref _weapon1WasPressed.Get(entity);
                weapon1WasPressed.Value = playerInput.Weapon1.WasPerformedThisFrame();
            }

            foreach (var entity in _weapon2_wasPressedFilter) {
                ref var weapon2WasPressed = ref _weapon2WasPressed.Get(entity);
                weapon2WasPressed.Value = playerInput.Weapon2.WasPerformedThisFrame();
            }

            foreach (var entity in _weapon3_wasPressedFilter) {
                ref var weapon3WasPressed = ref _weapon3WasPressed.Get(entity);
                weapon3WasPressed.Value = playerInput.Weapon3.WasPerformedThisFrame();
            }

            foreach (var entity in _weapon4_wasPressedFilter) {
                ref var weapon4WasPressed = ref _weapon4WasPressed.Get(entity);
                weapon4WasPressed.Value = playerInput.Weapon4.WasPerformedThisFrame();
            }
        }
    }
}
