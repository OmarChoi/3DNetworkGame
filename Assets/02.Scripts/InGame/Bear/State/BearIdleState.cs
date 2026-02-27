using UnityEngine;

public class BearIdleState : BearState
{
    private float _remainTime;
    private float _startPatrolTime;
    
    public BearIdleState(BearController controller) : base(controller) { }
    public override void Enter()
    {
        _remainTime = _startPatrolTime; // 대기 시간
    }

    public override void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            _controller.ChangeState<BearPatrolState>();
        }
    }
}