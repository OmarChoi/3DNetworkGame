using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;
        if (other.transform == _owner.transform) return;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_owner.Stat.Damage);
        }
    }
}