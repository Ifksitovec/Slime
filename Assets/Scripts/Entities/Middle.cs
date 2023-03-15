public class Middle : Enemy
{
    public override void Initialize(int level)
    {
        MaxHealth.Value = 50 + (level - 1) * 15;
        AttackPower.Value = 8 + level;
        AttackSpeed.Value = 30;
        HealthRegeneration.Value = 0;
        CriticalChance.Value = 0;
        Coin = 10 + level * 2;
        Health = MaxHealth.Value;
        base.Initialize(level);
    }
}
