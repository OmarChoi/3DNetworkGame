using System;
using Photon.Pun;
using UnityEngine;

public class PlayerAnimationAbility : PlayerAbility
{
    public static Action OnAttackAnimationEnd;

    private static readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int _attack1Trigger = Animator.StringToHash("Attack1Trigger");
    private static readonly int _attack2Trigger = Animator.StringToHash("Attack2Trigger");
    private static readonly int _attack3Trigger = Animator.StringToHash("Attack3Trigger");
    private static readonly int _deathTrigger = Animator.StringToHash("DeathTrigger");

    private CharacterController _characterController;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        PlayerAttackAbility.OnAttack += OnAttack;
        _owner.OnDeath += OnDeath;
        _owner.OnReset += OnReset;
    }

    private void OnDestroy()
    {
        PlayerAttackAbility.OnAttack -= OnAttack;
        _owner.OnDeath -= OnDeath;
        _owner.OnReset -= OnReset;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        UpdateAnimation();
    }
    
    private void UpdateAnimation()
    {
        Vector3 velocity = _characterController.velocity;
        velocity.y = 0;
        float speed = velocity.magnitude;

        float normalizedSpeed = speed / _owner.Stat.MoveSpeed;
        _animator.SetFloat(_moveSpeed, normalizedSpeed);
    }

    private EPlayerAttack GetRandomAttack()
    {
        return (EPlayerAttack)UnityEngine.Random.Range(0, (int)EPlayerAttack.Count);
    }

    private void OnAttack()
    {
        if (!_owner.PhotonView.IsMine) return;

        EPlayerAttack attack = GetRandomAttack();
        TriggerAttackAnimation(attack);
        _owner.PhotonView.RPC(nameof(RpcPlayAttackAnimation), RpcTarget.Others, (int)attack);
    }

    private void TriggerAttackAnimation(EPlayerAttack attack)
    {
        switch (attack)
        {
            case EPlayerAttack.Attack1:
                _animator.SetTrigger(_attack1Trigger);
                break;
            case EPlayerAttack.Attack2:
                _animator.SetTrigger(_attack2Trigger);
                break;
            case EPlayerAttack.Attack3:
                _animator.SetTrigger(_attack3Trigger);
                break;
        }
    }

    [PunRPC]
    private void RpcPlayAttackAnimation(int attackValue)
    {
        if (_owner.PhotonView.IsMine) return;
        TriggerAttackAnimation((EPlayerAttack)attackValue);
    }

    private void OnDeath()
    {
        _animator.SetTrigger(_deathTrigger);
    }

    private void OnReset()
    {
        _animator.Rebind();
        _animator.Update(0f);
    }

    private void OnAttackEnd()
    {
        if (!_owner.PhotonView.IsMine) return;
        OnAttackAnimationEnd?.Invoke();
    }
}
