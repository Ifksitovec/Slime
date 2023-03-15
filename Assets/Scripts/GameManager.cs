using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float _delayChecker = 1f;
    [SerializeField] private float _delayBeforeStart = 3f;
    private bool _isStartLevel = false;

    public Action<int> OnLevelChange;

    private void Start()
    {
        _levelManager.StartNewLevel += StartNewLevel;
        _enemies ??= new List<Enemy>();
        _player.SetEnemies(ref _enemies);
        StartCoroutine(StartFirstLevel());
    }

    private IEnumerator StartFirstLevel()
    {
        yield return new WaitForSeconds(_delayBeforeStart);
        _levelManager.NextLevel();
        StartCoroutine(CheckDiedAllEnemies());
    }

    private void OnDestroy()
    {
        _levelManager.StartNewLevel -= StartNewLevel;
        StopAllCoroutines();
    }

    public void KillEnemy(int coin, Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
        _player.AddCoin(coin);

        Destroy(enemy.gameObject);
    }

    private void StartNewLevel(int level, List<Enemy> enemies)
    {
        _isStartLevel = true;
        OnLevelChange?.Invoke(level);
        foreach (Enemy enemy in enemies)
        {
            enemy.Initialize(level);
        }
        _enemies.AddRange(enemies);
    }

    private IEnumerator CheckDiedAllEnemies()
    {
        while (!GameInfo.IsGameOver)
        {
            yield return new WaitForSeconds(_delayChecker);
            if (_enemies.Count == 0 && _isStartLevel)
            {
                _isStartLevel = false;
                _levelManager.StartNextLevel();
            }
        }
    }

    public int GetPlayerCoin()
    {
        return _player.Coin;
    }

    public void RemoveCoin(int coin)
    {
        _player.RemoveCoin(coin);
    }

    public HealthProperty GetMaxHealthProperty()
    {
        return _player.MaxHealth;
    }

    public RegenerationProperty GetRegenerationProperty()
    {
        return _player.HealthRegeneration;
    }

    public AttackPowerProperty GetAttackPowerProperty()
    {
        return _player.AttackPower;
    }

    public AttackSpeedProperty GetAttackSpeedProperty()
    {
        return _player.AttackSpeed;
    }

    public CriticalChanceProperty GetCriticalChanceProperty()
    {
        return _player.CriticalChance;
    }

    public void UpdateMaxPlayerHealth()
    {
        _player.UpdateHealth();
    }
}
