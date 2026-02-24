using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    public static Action OnAttack;
    
    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;
    }
    
    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && _owner.TryUseStamina(_owner.Stat.AttackStaminaUsage))
        {
            OnAttack?.Invoke();
        }
    }
}
