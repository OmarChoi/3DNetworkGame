using UnityEngine;

public class BearChaseState : BearState
{
    public BearChaseState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("ChaseEnter");
}