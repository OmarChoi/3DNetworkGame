using UnityEngine;

public class BearWaitAttackState : BearState
{
    private float _remainTime;
    private readonly float _attackCooldown = 2.0f;

    public BearWaitAttackState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("WaitAttackEnter");

    public override void Enter()
    {
        base.Enter();
        _remainTime = _attackCooldown;
        _controller.OnTargetLost += TransitionToReturn;
    }

    public override void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            _controller.ChangeState<BearAttackState>();
        }
    }

    public override void Exit()
    {
        _controller.OnTargetLost -= TransitionToReturn;
    }

    private void TransitionToReturn()
    {
        _controller.ChangeState<BearReturnState>();
    }
}
