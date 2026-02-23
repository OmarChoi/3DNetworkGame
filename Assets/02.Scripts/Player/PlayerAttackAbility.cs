using System;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    public static Action OnAttack;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnAttack?.Invoke();
        }
    }
}
