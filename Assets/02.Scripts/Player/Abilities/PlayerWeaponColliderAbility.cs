using UnityEngine;

public class PlayerWeaponColliderAbility : PlayerAbility
{
    [SerializeField] private Collider _collider;

    public void Start()
    {
        DeActiveCollider();
    }

    public void ActiveCollider()
    {
        Debug.Log("Activate collider");
        _collider.enabled = true;
    }

    public void DeActiveCollider()
    {
        Debug.Log("Deactivate collider");
        _collider.enabled = false;
    }
}