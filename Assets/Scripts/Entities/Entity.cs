using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Types;
using static EnemyAi;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private HealthProperty _maxHealth;
    [SerializeField] private RegenerationProperty _healthRegeneration;
    [SerializeField] private AttackPowerProperty _attackPower;
    [SerializeField] private AttackSpeedProperty _attackSpeed;
    [SerializeField] private CriticalChanceProperty _criticalChance;
    [SerializeField] private int _coin;
    [SerializeField] private GameObject _damageInfoPref;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected HealthSystemForDummies _healthSystem;
    [SerializeField] private Transform _attackPoint;

    protected Coroutine _attack;
    protected Coroutine _run;
    protected Coroutine _regenerate;

    public int Health
    {
        get { return _health; }
        protected set { _health = value; }
    }
    public HealthProperty MaxHealth
    {
        get { return _maxHealth; }
        protected set { _maxHealth = value; }
    }
    public RegenerationProperty HealthRegeneration
    {
        get { return _healthRegeneration; }
        protected set { _healthRegeneration = value; }
    }
    public AttackPowerProperty AttackPower
    {
        get { return _attackPower; }
        protected set { _attackPower = value; }
    }
    public AttackSpeedProperty AttackSpeed
    {
        get { return _attackSpeed; }
        protected set { _attackSpeed = value; }
    }
    public CriticalChanceProperty CriticalChance
    {
        get { return _criticalChance; }
        protected set { _criticalChance = value; }
    }
    public int Coin
    {
        get { return _coin; }
        protected set { _coin = value; }
    }
    public Transform AttackPoint { get { return _attackPoint; } }

    public virtual void Initialize()
    {
        UpdateHealth();
        StartRegenerate();
    }

    public virtual void StartAttack()
    {
        if (_attack == null)
        {
            _attack = StartCoroutine(Attack());
        }  
    }

    public virtual void StopAttack()
    {
        if (_attack != null)
        {
            StopCoroutine(_attack);
            _attack = null;
        }
    }

    public virtual void StartRun()
    {
        if (_run == null)
        {
            _run = StartCoroutine(Run());
        }
    }

    public virtual void StopRun()
    {
        if (_run != null)
        {
            StopCoroutine(_run);
            _run = null;
        }
    }

    public virtual void StartRegenerate()
    {
        if (_regenerate == null)
        {
            _regenerate = StartCoroutine(Regenerate());
        }
    }

    public virtual void StopRegenerate()
    {
        if (_regenerate != null)
        {
            StopCoroutine(_regenerate);
            _regenerate = null;
        }
    }

    public virtual void AddHealth(int count)
    {
        Health += count;
        Health = Mathf.Clamp(Health, 0, _maxHealth.Value);
        UpdateHealth();
        if (Health == 0)
        {
            Died();
        }
    }

    public virtual void Damage(int damage)
    {
        AddHealth(-damage);
        ShowDamage(damage);
    }

    public virtual void UpdateHealth()
    {
        _healthSystem.AddToMaximumHealth(MaxHealth.Value - _healthSystem.MaximumHealth);
        _healthSystem.AddToCurrentHealth(Health - _healthSystem.CurrentHealth);
        
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected int CalculateAttackDamage()
    {
        int res = AttackPower.Value;
        
        System.Random rand = new System.Random();
        int val = rand.Next(1, 100);

        if (val <= (CriticalChance.Value * 100f))
        {
            res *= 5;
        }

        return res;
    }

    protected virtual void Died() 
    {
        StopAllCoroutines();
    }

    protected virtual void ShowDamage(int damage)
    {
        GameObject damageInfo = Instantiate(_damageInfoPref, transform.position, Quaternion.identity);
        damageInfo.GetComponent<DamageObjectMover>().Initialize(damage);
        _anim.Play("Damage0");
    }

    protected virtual IEnumerator Regenerate()
    {
        while (true)
        {
            AddHealth(_healthRegeneration.Value / 60);
            yield return new WaitForSeconds(1f);
        }
    }

    protected abstract IEnumerator Attack();

    protected abstract IEnumerator Run();

    // Animation Event
    public void AlertObservers(string message)
    {
    }
}
