using Photon.Pun;
using UnityEngine;

public class PlayerAnimationAbility : PlayerAbility
{
    private static readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int _attack1Trigger = Animator.StringToHash("Attack1Trigger");
    private static readonly int _attack2Trigger = Animator.StringToHash("Attack2Trigger");
    private static readonly int _attack3Trigger = Animator.StringToHash("Attack3Trigger");

    private CharacterController _characterController;
    private Animator _animator;
    private bool _isAttacking;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _isAttacking = false;
        PlayerAttackAbility.OnAttack += OnAttack;
    }

    private void OnDestroy()
    {
        PlayerAttackAbility.OnAttack -= OnAttack;
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
        return (EPlayerAttack)Random.Range(0, (int)EPlayerAttack.Count);
    }

    private void OnAttack()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_isAttacking)
        {
            return;
        }

        EPlayerAttack attack = GetRandomAttack();
        _isAttacking = true;
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

    private void OnAttackEnd()
    {
        if (!_owner.PhotonView.IsMine) return;
        _isAttacking = false;
    }
}
