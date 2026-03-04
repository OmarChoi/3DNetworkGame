using UnityEngine;

public class BearReturnState : BearState
{
    public BearReturnState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("ReturnEnter");

    public override void Enter()
    {
        base.Enter();
        Agent.SetDestination(_controller.SpawnPosition);
        _controller.OnTargetDetected += TransitionToChase;
    }

    public override void Update()
    {
        if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            _controller.ChangeState<BearIdleState>();
        }
    }

    public override void Exit()
    {
        _controller.OnTargetDetected -= TransitionToChase;
        Agent.ResetPath();
    }

    private void TransitionToChase(Transform target)
    {
        _controller.ChangeState<BearChaseState>();
    }
}
