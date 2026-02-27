using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    public static Action OnAttack;

    private bool _isAttacking;

    private void Start()
    {
        if (!Owner.PhotonView.IsMine) return;
        PlayerAnimationAbility.OnAttackAnimationEnd += OnAttackEnd;
    }

    private void OnDestroy()
    {
        PlayerAnimationAbility.OnAttackAnimationEnd -= OnAttackEnd;
    }

    private void Update()
    {
        if (!Owner.PhotonView.IsMine) return;
        if (Owner.IsDead) return;
        if (_isAttacking) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && Owner.TryUseStamina(Owner.Stat.AttackStaminaUsage))
        {
            _isAttacking = true;
            OnAttack?.Invoke();
        }
    }

    private void OnAttackEnd()
    {
        _isAttacking = false;
    }
}
