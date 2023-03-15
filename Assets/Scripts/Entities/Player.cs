using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Entity
{
    [SerializeField] private GameObject _shellPref;
    protected List<Enemy> _enemies;

    private void Start()
    {
        Health = MaxHealth.Value;

        Initialize();
        StartAttack();
    }

    public virtual void UpgradeMaxHealth()
    {
        MaxHealth.Upgrade();
        UpdateHealth();
    }

    public virtual void UpgradeAttackPower()
    {
        AttackPower.Upgrade();
    }

    public virtual void UpgradeAttackSpeed()
    {
        AttackSpeed.Upgrade();
    }

    public virtual void UpgradeRegeneration()
    {
        HealthRegeneration.Upgrade();
    }

    public virtual void UpgradeCriticalChance()
    {
        CriticalChance.Upgrade();
    }

    public virtual void SetEnemies(ref List<Enemy> enemies)
    {
        _enemies = enemies;
    }

    protected override IEnumerator Attack()
    {
        while (!GameInfo.IsGameOver)
        {
            if (GetNearestEnemy(_enemies, out Enemy enemy))
            {
                _anim.Play("Attack");
                GameObject shell = Instantiate(_shellPref, AttackPoint.position, Quaternion.identity);
                shell.GetComponent<ShellMover>().Initialize(CalculateAttackDamage(), enemy);
            }

            yield return new WaitForSeconds(60f / (float)AttackSpeed.Value);
        }
    }

    protected virtual bool GetNearestEnemy(List<Enemy> enemies, out Enemy enemy)
    {
        if (enemies == null || enemies.Count == 0)
        {
            enemy = null;
            return false;
        }
        float distance = 0f;
        int minIndex = 0;
        for (int i = 0; i < enemies.Count; i++ )
        {
            if (i == 0)
            {
                distance = Vector3.Distance(transform.position, enemies[i].transform.position);
                minIndex = 0;
            }
            else
            {
                float newDistance = Vector3.Distance(transform.position, enemies[i].transform.position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    minIndex = i;
                }
            }
        }
        enemy = enemies[minIndex];
        return true;
    }

    public virtual void AddCoin(int coin)
    {
        Coin += coin;
    }

    public virtual void RemoveCoin(int coin)
    {
        Coin -= coin;
    }

    protected override void Died()
    {
        base.Died();
        GameInfo.IsGameOver = true;
    }
}
