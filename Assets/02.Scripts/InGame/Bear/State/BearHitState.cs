using UnityEngine;

public class BearHitState : BearState
{
    public BearHitState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("HitEnter");
}