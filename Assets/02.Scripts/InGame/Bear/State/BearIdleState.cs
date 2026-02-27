using UnityEngine;

public class BearIdleState : BearState
{
    private float _remainTime;
    private float _startPatrolTime;
    
    public BearIdleState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("IdleEnter");
    public override void Enter()
    {
        base.Enter();
        _remainTime = _startPatrolTime; // 대기 시간
    }

    public override void Update()
    {
        if (CheckChaseTransition()) return;
        if (CheckPatrolTransition()) return;
    }

    private bool CheckChaseTransition()
    {
        if (_controller.Target == null) return false;
        _controller.ChangeState<BearChaseState>();
        return true;
    }

    private bool CheckPatrolTransition()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime > 0) return false;
        _controller.ChangeState<BearPatrolState>();
        return true;
    }
}