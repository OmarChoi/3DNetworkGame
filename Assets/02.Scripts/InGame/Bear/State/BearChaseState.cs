using UnityEngine;

public class BearChaseState : BearState
{
    public BearChaseState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("ChaseEnter");

    public override void Enter()
    {
        base.Enter();
        _controller.OnTargetLost += TransitionToReturn;
    }

    public override void Update()
    {
        Agent.SetDestination(_controller.Target.position);

        if (_controller.CanAttack())
        {
            _controller.ChangeState<BearAttackState>();
        }
    }

    public override void Exit()
    {
        _controller.OnTargetLost -= TransitionToReturn;
        Agent.ResetPath();
    }

    private void TransitionToReturn()
    {
        _controller.ChangeState<BearReturnState>();
    }
}
