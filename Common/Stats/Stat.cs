using System.Collections;
using Sirenix.OdinInspector;

namespace Game
{
    public static class Stat
    {
        public const int Health = 0;
        public const int MaxHealth = 1;
        public const int HealthRegen = 2;
        public const int MoveSpeed = 3;
        public const int Mana = 4;
        public const int MaxMana = 5;
        public const int ManaRegen = 6;
        public const int Damage = 5;

        // Add this method to support ValueDropdown
        public static IEnumerable GetStatNames() =>
            new ValueDropdownList<int> {
                {"Health", Health},
                {"Max Health", MaxHealth},
                {"Health Regen", HealthRegen},
                {"Move Speed", MoveSpeed},
                {"Damage", Damage},
                {"Mana", Mana},
                {"Max Mana", MaxMana},
                {"Mana Regen", ManaRegen},
            };
    }
    
     
}