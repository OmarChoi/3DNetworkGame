using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    public static Action OnAttack;

    private bool _isAttacking;

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;
        PlayerAnimationAbility.OnAttackAnimationEnd += OnAttackEnd;
    }

    private void OnDestroy()
    {
        PlayerAnimationAbility.OnAttackAnimationEnd -= OnAttackEnd;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_isAttacking) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && _owner.TryUseStamina(_owner.Stat.AttackStaminaUsage))
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
