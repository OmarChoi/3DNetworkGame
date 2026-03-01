using UnityEngine;
using UnityEngine.AI;

public class BearPatrolState : BearState
{
    private readonly float _patrolRadius = 10.0f;

    public BearPatrolState(BearController controller) : base(controller) { }

    protected override int AnimTriggerHash => Animator.StringToHash("PatrolEnter");

    public override void Enter()
    {
        base.Enter();
        Agent.SetDestination(GetRandomDestination());
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

    private Vector3 GetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _patrolRadius;
        randomDirection += _controller.SpawnPosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _patrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return _controller.SpawnPosition;
    }

    private void TransitionToChase(Transform target)
    {
        _controller.ChangeState<BearChaseState>();
    }
}
