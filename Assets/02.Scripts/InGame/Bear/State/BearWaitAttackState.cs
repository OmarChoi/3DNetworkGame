using UnityEngine;

public class BearWaitAttackState : BearState
{
    public BearWaitAttackState(BearController controller) : base(controller) { }
    protected override int AnimTriggerHash => Animator.StringToHash("WaitAttackEnter");
}