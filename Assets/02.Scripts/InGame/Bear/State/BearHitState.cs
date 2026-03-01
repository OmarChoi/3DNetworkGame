using UnityEngine;

public class BearHitState : BearState
{
    public BearHitState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("HitEnter");

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
