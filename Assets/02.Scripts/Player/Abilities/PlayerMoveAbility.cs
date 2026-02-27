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

        bool isRunning = CheckRun();
        float moveSpeed = isRunning ? _owner.Stat.RunSpeed : _owner.Stat.MoveSpeed;
        movement *= moveSpeed;
        movement.y = _yVelocity;
        _characterController.Move(movement * Time.deltaTime);
    }

    private bool CheckRun()
    {
        float runStaminaUsage = _owner.Stat.RunStaminaUsage;

        if (Input.GetKey(KeyCode.LeftShift) && _owner.Stat.Stamina.Current > runStaminaUsage)
        {
            _owner.AddStamina(-runStaminaUsage *  Time.deltaTime);
            return true;
        }
        else
        {
            _owner.AddStamina(_owner.Stat.StaminaRecovery * Time.deltaTime);
            return false;
        }
    }
    
    private Vector3 GetMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0, v);
        return Camera.main.transform.TransformDirection(movement);
    }

    private void UpdateJump()
    {
        if (_characterController.isGrounded)
        {
            bool canJump = Input.GetKeyDown(KeyCode.Space) && _owner.TryUseStamina(_owner.Stat.JumpStaminaUsage);
            _yVelocity = canJump ? _owner.Stat.JumpPower : -1f;
        }
        _yVelocity -= GRAVITY * Time.deltaTime;
    }
}
