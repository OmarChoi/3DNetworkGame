using UnityEngine;

public class BearPatrolState : BearState
{
    public BearPatrolState(BearController controller) : base(controller) { }

    protected override int AnimTriggerHash => Animator.StringToHash("PatrolEnter");
}