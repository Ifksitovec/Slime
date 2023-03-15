using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR : Enemy
{
    public override void Initialize(int level)
    {
        MaxHealth.Value = 120 + (level - 1) * 50;
        AttackPower.Value = 13 + level;
        AttackSpeed.Value = 40;
        HealthRegeneration.Value = 0;
        CriticalChance.Value = 0;
        Coin = 100 + level * 10;
        Health = MaxHealth.Value;
        base.Initialize(level);
    }
}
