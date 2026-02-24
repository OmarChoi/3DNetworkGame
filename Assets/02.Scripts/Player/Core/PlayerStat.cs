using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public float MoveSpeed;
    public float RunSpeed;
    public float JumpPower;
    public float RotationSpeed;
    public float AttackSpeed;
    public float RunStaminaUsage;

    [SerializeField] private ResourceValue _stamina = new ResourceValue();
    [SerializeField] private ResourceValue _health = new ResourceValue();

    public ResourceValue Stamina
    {
        get
        {
            EnsureInitialized();
            return _stamina;
        }
    }

    public ResourceValue Health
    {
        get
        {
            EnsureInitialized();
            return _health;
        }
    }

    public void EnsureInitialized()
    {
        _stamina ??= new ResourceValue();
        _health ??= new ResourceValue();

        _stamina.ClampCurrent();
        _health.ClampCurrent();
    }

    public bool SetHealth(float value)
    {
        EnsureInitialized();
        return _health.SetCurrent(value);
    }

    public bool SetStamina(float value)
    {
        EnsureInitialized();
        return _stamina.SetCurrent(value);
    }

    public bool AddStamina(float delta)
    {
        EnsureInitialized();
        return _stamina.Add(delta);
    }
}
