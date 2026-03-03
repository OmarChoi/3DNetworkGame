using UnityEngine;

public class BearDetectAbility : BearAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            Owner.SetTarget(player.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == Owner.Target)
        {
            Owner.ClearTarget();
        }
    }
}
