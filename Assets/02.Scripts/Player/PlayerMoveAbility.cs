using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private const float GRAVITY = 30.0f;
    private float _yVelocity = 0f;
    
    private CharacterController _characterController;
    private bool _isMine;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        _yVelocity = 0f;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        Vector3 movement = GetMovement();
        UpdateJump();

        movement.y = _yVelocity;
        _characterController.Move(movement * Time.deltaTime);
    }

    private Vector3 GetMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0, v);
        return transform.TransformDirection(movement) * _owner.Stat.MoveSpeed;
    }

    private void UpdateJump()
    {
        if (_characterController.isGrounded)
        {
            _yVelocity = Input.GetKey(KeyCode.Space) ? _owner.Stat.JumpPower : -1f;
        }
        _yVelocity -= GRAVITY * Time.deltaTime;
    }
}
