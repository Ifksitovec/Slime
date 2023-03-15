public class Senior : Enemy
{
    public override void Initialize(int level)
    {
        MaxHealth.Value = 80 + (level - 1) * 20;
        AttackPower.Value = 13 + level;
        AttackSpeed.Value = 40;
        HealthRegeneration.Value = 0;
        CriticalChance.Value = 0;
        Coin = 20 + level * 3;
        Health = MaxHealth.Value;
        base.Initialize(level);
    }
}
