using UnityEngine;

public class BearAttackState : BearState
{
    public BearAttackState(BearController controller) : base(controller) { }

    private static readonly int _animTriggerHash = Animator.StringToHash("AttackEnter");
    protected override int AnimTriggerHash => _animTriggerHash;

    public override void Enter()
    {
        _controller.OnTargetLost += TransitionToReturn;
        _controller.OnAttackAnimationEnd += OnAttackEnd;
        base.Enter();
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
