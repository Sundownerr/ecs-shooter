namespace Game
{
    public static class StatsExtentions
    {
        public static float Health(this Stats<float> stats) => stats.ValueOf(Stat.Health);
        public static float MaxHealth(this Stats<float> stats) => stats.ValueOf(Stat.MaxHealth);
        public static float HealthRegen(this Stats<float> stats) => stats.ValueOf(Stat.HealthRegen);
        public static float MoveSpeed(this Stats<float> stats) => stats.ValueOf(Stat.MoveSpeed);

        public static void IncreaseHealth(this Stats<float> stats, float value) =>
            stats.Increase(Stat.Health, value);

        public static void IncreaseMaxHealth(this Stats<float> stats, float value) =>
            stats.Increase(Stat.MaxHealth, value);

        public static void IncreaseHealthRegen(this Stats<float> stats, float value) =>
            stats.Increase(Stat.HealthRegen, value);

        public static void IncreaseMoveSpeed(this Stats<float> stats, float value) =>
            stats.Increase(Stat.MoveSpeed, value);

        public static void DecreaseHealth(this Stats<float> stats, float value) =>
            stats.Decrease(Stat.Health, value);

        public static void DecreaseMaxHealth(this Stats<float> stats, float value) =>
            stats.Decrease(Stat.MaxHealth, value);

        public static void DecreaseHealthRegen(this Stats<float> stats, float value) =>
            stats.Decrease(Stat.HealthRegen, value);

        public static void DecreaseMoveSpeed(this Stats<float> stats, float value) =>
            stats.Decrease(Stat.MoveSpeed, value);

        public static void SetHealth(this Stats<float> stats, float value) =>
            stats.Set(Stat.Health, value);

        public static void SetMaxHealth(this Stats<float> stats, float value) =>
            stats.Set(Stat.MaxHealth, value);

        public static void SetHealthRegen(this Stats<float> stats, float value) =>
            stats.Set(Stat.HealthRegen, value);

        public static void SetMoveSpeed(this Stats<float> stats, float value) =>
            stats.Set(Stat.MoveSpeed, value);
    }
}