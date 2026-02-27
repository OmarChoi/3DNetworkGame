using UnityEngine;

public class BearIdleState : BearState
{
    private float _enterTime;
    private float _startPatrolTime;
    
    public BearIdleState(BearController controller) : base(controller) { }
    public virtual void Enter()
    {
        _enterTime = Time.time;
    }

    public virtual void Update()
    {
        _enterTime -= Time.deltaTime;
        if (_enterTime <= 0)
        {
            _enterTime = Time.time;
        }
    }

    public virtual void Exit()
    {
        
    }
}