using Photon.Pun;
using UnityEngine;

public class BearAttackAbility : BearAbility
{
    public void Attack()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (Owner.Target == null) return;
        if (Owner.Target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(Owner.Stats.Damage, -1);
        }
    }
}
