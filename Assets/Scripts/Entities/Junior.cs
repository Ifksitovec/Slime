using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junior : Enemy
{
    public override void Initialize(int level)
    {
        MaxHealth.Value = 25 + (level - 1) * 10;
        AttackPower.Value = 4 + level;
        AttackSpeed.Value = 20;
        HealthRegeneration.Value = 0;
        CriticalChance.Value = 0;
        Coin = 1 + level;
        Health = MaxHealth.Value;
        base.Initialize(level);
    }
}
