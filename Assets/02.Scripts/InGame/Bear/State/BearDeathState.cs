using UnityEngine;

public class BearDeathState : BearState
{
    private float _remainTime;
    private readonly float _destroyDelay = 3.0f;

    public BearDeathState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("DeathEnter");

    public override void Enter()
    {
        _controller.Animator.SetTrigger(AnimTriggerHash);
        _controller.Agent.enabled = false;
        _remainTime = _destroyDelay;
    }

    public override void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            Object.Destroy(_controller.gameObject);
        }
    }
}
