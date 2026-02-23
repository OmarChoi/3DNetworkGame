using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private CharacterController _characterController;
    private Animator _animator;
    private PlayerStat _stat;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stat = GetComponent<PlayerStat>();
    }

    private void Update()
    {
        UpdateAnimation();
    }
    
    private void UpdateAnimation()
    {
        float speed = new Vector3
        (
            _characterController.velocity.x,
            0,
            _characterController.velocity.z
        ).magnitude;

        float normalizedSpeed = speed / _stat.MoveSpeed;
        _animator.SetFloat(_moveSpeed, normalizedSpeed);
    }

}
