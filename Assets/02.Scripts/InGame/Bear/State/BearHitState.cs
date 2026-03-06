using UnityEngine;

public class BearHitState : BearState
{
    public BearHitState(BearController controller) : base(controller) { }
    private static readonly int _animTriggerHash = Animator.StringToHash("HitEnter");
    protected override int AnimTriggerHash => _animTriggerHash;

    public override void Enter()
    {
        base.Enter();
        _controller.OnHitAnimationEnd += TransitionToChase;
    }

    public override void Exit()
    {
        _controller.OnHitAnimationEnd -= TransitionToChase;
    }

    private void TransitionToChase()
    {
        _controller.ChangeState<BearChaseState>();
    }
}
