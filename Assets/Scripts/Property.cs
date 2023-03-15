using System;
using UnityEngine;

public enum TypeProperty
{
    Health = 1,
    Regeneration,
    AttackPower,
    AttackSpeed,
    CriticalChance
}

[Serializable]
public abstract class Property<T>
{
    [SerializeField] private T _value;
    [SerializeField] private int _price;

    public T Value
    {
        get { return _value; }
        set { _value = value; }
    }
    public int Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public abstract void Upgrade();

    public override string ToString()
    {
        return _value.ToString();
    }
}

[Serializable]
public class HealthProperty : Property<int>
{
    public override void Upgrade()
    {
        Value += 10;
        Price++; 
    }
}

[Serializable]
public class RegenerationProperty : Property<int>
{
    public override void Upgrade()
    {
        Value++;
        Price += 5;
    }
}

[Serializable]
public class AttackPowerProperty : Property<int>
{
    public override void Upgrade()
    {
        Value += 10;
        Price++;
    }
}

[Serializable]
public class AttackSpeedProperty : Property<int>
{
    public override void Upgrade()
    {
        Value++;
        Price += 5;
    }
}

[Serializable]
public class CriticalChanceProperty : Property<float>
{
    public override void Upgrade()
    {
        Value += 0.01f;
        Price += 10;
    }

    public override string ToString()
    {
        return string.Format("{0:F2}", Value);
    }
}
