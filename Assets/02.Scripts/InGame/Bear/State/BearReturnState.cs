using UnityEngine;

public class BearReturnState : BearState
{
    public BearReturnState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("ReturnEnter");
}