using System;
using System.Collections;
using UnityEngine;

public enum TypeEnemy
{
    Junior = 1,
    Middle,
    Senior,
    HR
}

public abstract class Enemy : Entity
{
    [SerializeField] private float _speedMove;
    [SerializeField] private float _attackDistance;
    protected Vector3? _startPosition = null;
    protected Player _player;
    protected bool _isArroundThePlayer = false;
    protected GameManager _gameManager;
    protected Action<int, Enemy> OnDied;

    public virtual void Initialize(int level)
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        OnDied += _gameManager.KillEnemy;
        Initialize();
        StartRun();
    }

    public override void StartRun()
    {
        if (_player == null)
        {
            _player = FindPlayer();
        }

        if (_startPosition == null)
        {
            _startPosition = transform.position;
        }
        base.StartRun();
    }

    protected override IEnumerator Run()
    {
        while (!GameInfo.IsGameOver && !_isArroundThePlayer)
        {
            float changeDist = _speedMove * Time.fixedDeltaTime / (60f);
            Vector3 playerPos = _player.transform.position;
            float dx = Mathf.Lerp(_startPosition.Value.x, playerPos.x, changeDist) - _startPosition.Value.x;
            float dz = Mathf.Lerp(_startPosition.Value.z, playerPos.z, changeDist) - _startPosition.Value.z;
            transform.position = transform.position + new Vector3(dx, 0, dz);
            FindPlayerArround();
            yield return new WaitForFixedUpdate();
        }
        _run = null;
        StartAttack();
    }

    protected override IEnumerator Attack()
    {
        while (!GameInfo.IsGameOver)
        {
            _player.Damage(CalculateAttackDamage());
            _anim.Play("Attack");
            yield return new WaitForSeconds(60f / (float)AttackSpeed.Value);
        }
        _attack = null;
    }

    protected override void Died()
    {
        OnDied?.Invoke(Coin, this);
        base.Died();
    }

    protected virtual Player FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected virtual void FindPlayerArround()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Ray ray = new Ray(transform.position, Vector3.left);
        if (Physics.SphereCast(ray, _attackDistance, out RaycastHit hitInfo, _attackDistance, layerMask))
        {
            _isArroundThePlayer = true;
        }
    }
}
