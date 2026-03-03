using UnityEngine;

public class BearReturnState : BearState
{
    public BearReturnState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("ReturnEnter");

    public override void Enter()
    {
        base.Enter();
        Agent.SetDestination(_controller.SpawnPosition);
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
        Agent.ResetPath();
    }
}
