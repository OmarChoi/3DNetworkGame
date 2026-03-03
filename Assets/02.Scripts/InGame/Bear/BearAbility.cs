using UnityEngine;

public class BearAbility : MonoBehaviour
{
    protected BearController Owner { get; private set; }

    protected virtual void Awake()
    {
        Owner = GetComponentInParent<BearController>();
    }
}