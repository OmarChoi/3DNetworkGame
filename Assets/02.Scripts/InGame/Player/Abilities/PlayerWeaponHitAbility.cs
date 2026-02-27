using Photon.Pun;
using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!Owner.PhotonView.IsMine) return;
        if (other.transform == Owner.transform) return;
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(Owner.Stat.Damage, actorNumber);
        }
    }
}