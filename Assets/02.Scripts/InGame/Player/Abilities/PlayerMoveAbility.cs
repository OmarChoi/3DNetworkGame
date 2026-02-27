using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private const float GRAVITY = 30.0f;
    private const float FALL_RESPAWN_HEIGHT = -10f;
    private float _yVelocity = 0f;
    
    private CharacterController _controller;

    protected override void Awake()
    {
        base.Awake();
        _controller = Owner.Controller;
    }
    
    private void Start()
    {
        _yVelocity = 0f;
    }

    private void Update()
    {
        if (!Owner.PhotonView.IsMine) return;
        if (Owner.IsDead) return;
        Vector3 movement = GetMovement();
        UpdateJump();

        bool isRunning = CheckRun();
        float moveSpeed = isRunning ? Owner.Stat.RunSpeed : Owner.Stat.MoveSpeed;
        movement *= moveSpeed;
        movement.y = _yVelocity;
        _controller.Move(movement * Time.deltaTime);

        CheckFallRespawn();
    }

    private bool CheckRun()
    {
        float runStaminaUsage = Owner.Stat.RunStaminaUsage;

        if (Input.GetKey(KeyCode.LeftShift) && Owner.Stat.Stamina.Current > runStaminaUsage)
        {
            Owner.AddStamina(-runStaminaUsage *  Time.deltaTime);
            return true;
        }
        else
        {
            Owner.AddStamina(Owner.Stat.StaminaRecovery * Time.deltaTime);
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
        if (_controller.isGrounded)
        {
            bool canJump = Input.GetKeyDown(KeyCode.Space) && Owner.TryUseStamina(Owner.Stat.JumpStaminaUsage);
            _yVelocity = canJump ? Owner.Stat.JumpPower : -1f;
        }
        _yVelocity -= GRAVITY * Time.deltaTime;
    }

    private void CheckFallRespawn()
    {
        if (transform.position.y >= FALL_RESPAWN_HEIGHT) return;
        if (CharacterSpawner.Instance == null) return;

        CharacterSpawner.Instance.StartRespawn(Owner);
    }
}
