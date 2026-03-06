using Photon.Pun;
using UnityEngine;

public class BearDeathState : BearState
{
    private float _remainTime;
    private readonly float _destroyDelay = 3.0f;

    public BearDeathState(BearController controller) : base(controller) { }
    private static readonly int _animTriggerHash = Animator.StringToHash("DeathEnter");
    protected override int AnimTriggerHash => _animTriggerHash;

    public override void Enter()
    {
        base.Enter();
        _controller.Agent.enabled = false;
        _remainTime = _destroyDelay;
    }

    public override void Update()
    {
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            PhotonNetwork.Destroy(_controller.photonView);
        }
    }
}
