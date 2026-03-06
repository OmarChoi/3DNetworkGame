using UnityEngine;

public class BearIdleState : BearState
{
    private float _remainTime;
    private readonly float _startPatrolTime = 3.0f;

    public BearIdleState(BearController controller) : base(controller) { }
    private static readonly int _animTriggerHash = Animator.StringToHash("IdleEnter");
    protected override int AnimTriggerHash => _animTriggerHash;

    public override void Enter()
    {
        base.Enter();
        _remainTime = _startPatrolTime;
        _controller.OnTargetDetected += TransitionToChase;
    }

    public override void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            _controller.ChangeState<BearPatrolState>();
        }
    }

    public override void Exit()
    {
        _controller.OnTargetDetected -= TransitionToChase;
    }

    private void TransitionToChase(Transform target)
    {
        _controller.ChangeState<BearChaseState>();
    }
}
