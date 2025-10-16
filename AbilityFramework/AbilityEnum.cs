namespace Game
{
    public enum Position
    {
        UserTransform = 0,
        TargetTransform = 1,
        CustomTransform = 2,
        CustomPosition = 3,
        NearestNavMeshPoint = 5,
    }

    public enum Rotation
    {
        UserTransform = 0,
        TargetTransform = 1,
        CustomTransform = 2,
        CustomRotation = 3,
        TowardsTarget = 4,
    }

    public enum Direction
    {
        TargetAwayFromUser = 0,
        TargetTowardsUser = 1,
        UserAwayFromTarget = 6,
        UserTowardsTarget = 4,
        CustomWorldDirection = 2,
        CustomLocalDirection = 3,
        TransformForward = 5,
    }

    public enum AbilityTargetType
    {
        UserTarget = 0,
        AllInAOE = 1,
        LimitedInAOE = 2,
        FromScript = 3,
        FromTargetProvider = 4,
    }

    public enum TargetType
    {
        Self = 0, Other = 1, Target = 2,
    }

    public enum TargetTransform
    {
        UserTransform = 0, TargetTransform = 1, OtherTransform = 2,
    }

    public enum TargetRigidbody
    {
        UserRigidbody = 0, TargetRigidbody = 1, OtherRigidbody = 2,
    }

    public enum RigidbodyActionType
    {
        AddForce = 0, AddExplosionForce = 1, SetKinematic = 2,
    }

    public enum TriggerInteractionType
    {
        Enter = 0, Stay = 1, Exit = 2,
    }



    public enum UseAction
    {
        CreateGameObject = 1,
        PlayMMFeedback = 0,
        RotateTransform = 13,
        MoveTransform = 18,
        NavMeshAgent = 3,
        Animator = 10,
        Particle = 11,
        Rigidbody = 15,
        DestroyUser = 2,
        Damage = 12,
        SetGameObjectActive = 6,
        SetAbilityActivated = 9,
        WeaponTrigger = 14,
        ChangeYellowCubes = 16,
        CreateEnemy = 17,
        LineRenderer = 19,
        ChangeMana = 20,
        RegenerateFullHealth = 21,
        RegenerateFullMana = 22,
        Dash = 23,
        Sprint = 24,
        ChangeFloatStatValue = 25,
        AddFloatStatModifier = 26,
        RemoveFloatStatModifier = 27,
        PlayStaticParticle = 28,
        ReturnToPool = 29,
        DestroyGameObject = 30,
        ChangeComponentEnabled = 31,
        ChangeWorldForward = 32,
    }

    public enum AbilityConditionType
    {
        DistanceToTargetLessThan = 0,
        DistanceToTargetGreaterThan = 1,
        UserTeam = 2,
        NotOnCooldown = 3,
        Activated = 4,
        HaveTarget = 5,
        Trigger = 6,
        WeaponTrigger = 7,
        CheckSphere = 8,
        CheckRaycastHit = 9,
        PrimaryAbilityPressed = 10,
        SecondaryAbilityPressed = 11,
        SprintPressed = 12,
        PrimaryAttackPressed = 13,
        SecondaryAttackPressed = 14,
        DashPressed = 15,
        PrimaryAbilityReleased = 16,
        SecondaryAbilityReleased = 17,
        SprintReleased = 18,
        PrimaryAttackReleased = 19,
        SecondaryAttackReleased = 20,
        PrimaryAbilityWasPressed = 21,
        SecondaryAbilityWasPressed = 22,
        SprintWasPressed = 23,
        PrimaryAttackWasPressed = 24,
        SecondaryAttackWasPressed = 25,
        DashWasPressed = 26,
        Mana = 27,
        HasBeenDamaged = 28,
        VelocityGreaterThan = 29,
        VelocityLowerThan = 30,
        GameObjectActive = 31,
        YellowCubes = 32,
        Health = 33,
        TotalYellowCubes = 34,
    }

    public enum WeaponTriggerCondition { Pulled = 0, Released = 1, }

    public enum AbilityCooldownType { Cooldown = 0, Recharge = 1, }

    public enum ManaComparisonType { LessThan = 0, MoreThan = 1, }

    public enum DamageCheckType
    {
        Any = 0,           // Check if damaged at all
        ByAmount = 1,      // Check if damaged by at least N amount
        ByPercent = 2,     // Check if damaged by at least N% of MaxHealth
    }
}
