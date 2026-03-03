using UnityEngine;

public class BearAttackState : BearState
{
    public BearAttackState(BearController controller) : base(controller) { }

    protected override int AnimTriggerHash => Animator.StringToHash("AttackEnter");

    public override void Enter()
    {
        base.Enter();
        _controller.OnTargetLost += TransitionToReturn;
        _controller.OnAttackAnimationEnd += OnAttackEnd;
    }

    public override void Exit()
    {
        _controller.OnTargetLost -= TransitionToReturn;
        _controller.OnAttackAnimationEnd -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        if (_controller.CanAttack())
        {
            _controller.ChangeState<BearWaitAttackState>();
        }
        else
        {
            _controller.ChangeState<BearChaseState>();
        }
    }

    private void TransitionToReturn()
    {
        _controller.ChangeState<BearReturnState>();
    }
}
