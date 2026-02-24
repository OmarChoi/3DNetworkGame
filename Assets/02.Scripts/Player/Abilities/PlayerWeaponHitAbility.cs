using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _owner.transform) return;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Damage");
            damageable.TakeDamage(_owner.Stat.Damage);
        }
    }
}