using UnityEngine;

public class BearAttackState : BearState
{
    public BearAttackState(BearController controller) : base(controller) { }

    protected override int AnimTriggerHash =>  Animator.StringToHash("AttackEnter");
}