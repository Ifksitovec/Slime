using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _numbBossLevel = 5;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<TypePrefEnemy> _listPrefs;
    [SerializeField] private float _delayStartLevel = 3f;
    
    public Action<int, List<Enemy>> StartNewLevel;
    private int _currenLevel = 0;
    System.Random _rand;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void StartNextLevel()
    {
        StartCoroutine(AwaitDelay());
    }

    public void NextLevel()
    {
        _currenLevel++;
        StartNewLevel?.Invoke(_currenLevel, SpawnNewEnemies());
    }

    private List<Enemy> SpawnNewEnemies()
    {
        List<Enemy> list = new List<Enemy>();

        if (_currenLevel % _numbBossLevel == 0)
        {
            list.Add(SpawnEnemy(TypeEnemy.HR));
        }
        else
        {
            int count = 2 + _currenLevel / _numbBossLevel;

            Debug.Log(count);
            if (_rand == null)
            {
                _rand = new System.Random();
            }
            
            for (int i = 0; i < count; i++)
            {
                int randNumb = _rand.Next(1, _currenLevel);
                if (randNumb <= _numbBossLevel)
                {
                    list.Add(SpawnEnemy(TypeEnemy.Junior));
                }
                else if (randNumb > _numbBossLevel && randNumb <= _numbBossLevel * 2)
                {
                    list.Add(SpawnEnemy(TypeEnemy.Middle));
                }
                else
                {
                    list.Add(SpawnEnemy(TypeEnemy.Senior));
                }
            }
        }
        return list;
    }

    private Enemy SpawnEnemy(TypeEnemy typeEnemy)
    {
        Vector3 pos = _spawnPoint.position + new Vector3(_rand.Next(-1, 1), 0, _rand.Next(-2, 2));
        GameObject enemyObject = Instantiate(GetPref(typeEnemy), pos, _spawnPoint.rotation);

        return enemyObject.GetComponent<Enemy>();
    }

    private GameObject GetPref(TypeEnemy typeEnemy)
    {
        foreach (var pref in _listPrefs)
        {
            if (pref.TEnemy == typeEnemy)
            {
                return pref.Pref;
            }
        }
        return null;
    }

    private IEnumerator AwaitDelay()
    {
        yield return new WaitForSeconds(_delayStartLevel);
        NextLevel();
    }
}

[Serializable]
public class TypePrefEnemy
{
    public TypeEnemy TEnemy;
    public GameObject Pref;
}
