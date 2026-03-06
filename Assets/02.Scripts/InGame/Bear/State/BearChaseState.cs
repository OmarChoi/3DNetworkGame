using UnityEngine;

public class BearChaseState : BearState
{
    public BearChaseState(BearController controller) : base(controller) { }
    private static readonly int _animTriggerHash = Animator.StringToHash("ChaseEnter");
    protected override int AnimTriggerHash => _animTriggerHash;
  
    private const float PathUpdateInterval = 0.1f;
    private float _pathUpdateTimer;

    public override void Enter()
    {
        base.Enter();
        _controller.OnTargetLost += TransitionToReturn;
    }

    public override void Update()
    {
        UpdatePath();
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

    private void UpdatePath()
    {
        _pathUpdateTimer -= Time.deltaTime;
        if (_pathUpdateTimer <= 0f)
        {
            _pathUpdateTimer = PathUpdateInterval;
            Agent.SetDestination(_controller.Target.position);
        }
    }
    
    private void TransitionToReturn()
    {
        _controller.ChangeState<BearReturnState>();
    }
}
