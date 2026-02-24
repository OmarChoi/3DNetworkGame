using Photon.Pun;
using UnityEngine;

public class PlayerAnimationAbility : PlayerAbility
{
    private static readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int _attackIndex = Animator.StringToHash("AttackIndex");

    private CharacterController _characterController;
    private Animator _animator;
    private EPlayerAttack _nextAttack;

    private bool _isMine;
    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
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
        EPlayerAttack attack = GetRandomAttack();
        if (_animator.GetInteger(_attackIndex) != (int)EPlayerAttack.None)
        {
            if (_nextAttack != EPlayerAttack.None) return;
            _nextAttack = attack;
            return;
        }
        _animator.SetInteger(_attackIndex, (int)attack);
    }

    private void OnAttackEnd()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_nextAttack != EPlayerAttack.None)
        {
            _animator.SetInteger(_attackIndex, (int)_nextAttack);
            _nextAttack = EPlayerAttack.None;
            return;
        }
        _animator.SetInteger(_attackIndex, (int)EPlayerAttack.None);
    }
}
