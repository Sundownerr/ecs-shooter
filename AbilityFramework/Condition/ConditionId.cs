using System.Collections.Generic;

namespace Ability.Identifications
{
    public static class ConditionId
    {
        public const int UserTeam = 1;
        public const int NotOnCooldown = 2;
        public const int Activated = 3;
        public const int HaveTarget = 4;
        public const int Trigger = 5;
        public const int WeaponTrigger = 6;
        public const int CheckSphere = 7;
        public const int CheckRaycastHit = 8;
        public const int Mana = 9;
        public const int HasBeenDamaged = 10;
        public const int VelocityGreaterThan = 11;
        public const int VelocityLowerThan = 12;
        public const int GameObjectActive = 13;
        public const int YellowCubes = 14;
        public const int Health = 15;
        public const int Total = 16;
        public const int GatheredOnLevel = 17;

        public const int DistanceToTarget = 100;
        public const int Velocity = 101;
        public const int InputPressed = 102;
        public const int InputReleased = 103;
        public const int InputWasPressed = 104;
        public const int Stats = 105;
        public const int Physics = 106;

        public const int PrimaryAttack = 1000;
        public const int SecondaryAttack = 1001;
        public const int PrimaryAbility = 1002;
        public const int SecondaryAbility = 1003;
        public const int Sprint = 1004;
        public const int Dash = 1005;

        public const int LessThan = 2000;
        public const int GreaterThan = 2001;

        public static readonly Dictionary<int, string> ToName = new() {
            {UserTeam, "User Team"},
            {NotOnCooldown, "Not On Cooldown"},
            {Activated, "Activated"},
            {HaveTarget, "Have Target"},
            {Trigger, "Trigger"},
            {WeaponTrigger, "Weapon Trigger"},

            {CheckSphere, "Check Sphere"},
            {CheckRaycastHit, "Check Raycast Hit"},

            {Mana, "Mana"},
            {Health, "Health"},
            {HasBeenDamaged, "Has Been Damaged"},
            {YellowCubes, "Yellow Cubes"},
            {Total, "Total"},
            {GatheredOnLevel, "Gathered On Level"},

            {VelocityGreaterThan, "Velocity Greater Than"},
            {VelocityLowerThan, "Velocity Lower Than"},
            {Velocity, "Velocity"},

            {GameObjectActive, "GameObject Active"},
            {DistanceToTarget, "Distance To Target"},
            {InputPressed, "Input Pressed"},
            {InputReleased, "Input Released"},
            {InputWasPressed, "Input Was Pressed"},

            {PrimaryAttack, "Primary Attack"},
            {SecondaryAttack, "Secondary Attack"},
            {PrimaryAbility, "Primary Ability"},
            {SecondaryAbility, "Secondary Ability"},
            {Sprint, "Sprint"},
            {Dash, "Dash"},

            {LessThan, "Less Than"},
            {GreaterThan, "Greater Than"},

            {Stats, "Stats"},
            {Physics, "Physics"},
        };
    }
}