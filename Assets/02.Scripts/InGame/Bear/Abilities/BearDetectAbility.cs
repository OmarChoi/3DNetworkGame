using UnityEngine;

public class BearDetectAbility : BearAbility
{
    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    public void ActiveCollider()
    {
        _collider.enabled = true;
    }

    public void DeActiveCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            DeActiveCollider();
            Owner.SetTarget(player.transform);
        }
    }
}