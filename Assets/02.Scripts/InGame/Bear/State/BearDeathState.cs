using UnityEngine;

public class BearDeathState : BearState
{
    public BearDeathState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("DeathEnter");
}