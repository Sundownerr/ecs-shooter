using System.Collections.Generic;

namespace Ability.Identifications
{
    public static class ActionId
    {
        public const int Create = 0;
        public const int Destroy = 1;
        public const int Enable = 2;
        public const int GameObject = 3;
        public const int Entity = 4;
        public const int Component = 5;
        public const int Activate = 6;
        public const int NavMeshAgent = 7;
        public const int Play = 8;
        public const int ParticleSystem = 9;
        public const int StaticParticleSystem = 10;
        public const int MMFPlayer = 11;
        public const int Disable = 12;
        public const int Deactivate = 13;
        public const int ReturnToPool = 14;
        public const int FloatStats = 15;
        public const int ChangeStatValue = 16;
        public const int AddStatModifier = 17;
        public const int RemoveStatModifier = 18;
        public const int Animator = 19;
        public const int ChangeComponentEnabled = 20;
        public const int ChangeFloatStat = 21;
        public const int ChangeMana = 22;
        public const int ChangeYellowCubes = 23;
        public const int Damage = 24;
        public const int Dash = 25;
        public const int LineRenderer = 26;
        public const int Move = 27;
        public const int RegenerateFullHealth = 28;
        public const int RegenerateFullMana = 29;
        public const int Rigidbody = 30;
        public const int Rotate = 31;
        public const int SetAbilityActivated = 32;
        public const int SetActive = 33;
        public const int Sprint = 34;
        public const int WeaponTrigger = 35;
        public const int ComponentEnabled = 36;
        public const int FloatStat = 37;
        public const int Mana = 38;
        public const int YellowCubes = 39;
        public const int Enemy = 40;
        public const int User = 42;
        public const int Transform = 44;
        public const int Health = 45;
        public const int Ability = 47;
        public const int Resource = 48;
        public const int ChangeWorldForward = 49;
        
        public static readonly Dictionary<int, string> ToName = new()
        {
            {Create, "Create"},
            {Destroy, "Destroy"},
            {Enable, "Enable"},
            {GameObject, "GameObject"},
            {Entity, "Entity"},
            {Component, "Component"},
            {Activate, "Activate"},
            {NavMeshAgent, "NavMesh Agent"},
            {Play, "Play"},
            {ParticleSystem, "Particle System"},
            {StaticParticleSystem, "Static Particle System"},
            {MMFPlayer, "MMF Player"},
            {Disable, "Disable"},
            {Deactivate, "Deactivate"},
            {ReturnToPool, "Return To Pool"},
            {FloatStats, "Float Stats"},
            {ChangeStatValue, "Change Value"},
            {AddStatModifier, "Add Modifier"},
            {RemoveStatModifier, "Remove Modifier"},
            {Animator, "Animator"},
            {ChangeComponentEnabled, "Change Component Enabled"},
            {ChangeFloatStat, "Change Float Stat"},
            {ChangeMana, "Change Mana"},
            {ChangeYellowCubes, "Change Yellow Cubes"},
            {Damage, "Damage"},
            {Dash, "Dash"},
            {LineRenderer, "Line Renderer"},
            {Move, "Move"},
            {RegenerateFullHealth, "+ Full Health"},
            {RegenerateFullMana, "+ Full Mana"},
            {Rigidbody, "Rigidbody"},
            {Rotate, "Rotate"},
            {SetAbilityActivated, "Set Ability Activated"},
            {SetActive, "Set Active"},
            {Sprint, "Sprint"},
            {WeaponTrigger, "Weapon Trigger"},
            {ComponentEnabled, "Component Enabled"},
            {FloatStat, "Float Stat"},
            {Mana, "Mana"},
            {YellowCubes, "Yellow Cubes"},
            {Enemy, "Enemy"},
            {User, "User"},
            {Transform, "Transform"},
            {Health, "Health"},
            {Ability, "Ability"},
            {Resource, "Resource"},
            {ChangeWorldForward, "Change World Forward"},
        };
    }
}
