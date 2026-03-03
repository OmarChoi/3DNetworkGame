using System;
using UnityEngine;

[Serializable]
public class BearStat
{
    public int AttackDistance;
    public float Damage;
    
    [SerializeField] private ResourceValue _health = new ResourceValue();

    public ResourceValue Health
    {
        get
        {
            _health ??= new ResourceValue();
            return _health;
        }
    }
    
}