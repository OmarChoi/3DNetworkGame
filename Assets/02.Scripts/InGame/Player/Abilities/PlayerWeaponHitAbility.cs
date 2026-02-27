using Photon.Pun;
using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;
        if (other.transform == _owner.transform) return;
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_owner.Stat.Damage, actorNumber);
        }
    }
}