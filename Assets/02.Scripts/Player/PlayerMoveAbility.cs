using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    private const float GRAVITY = 30.0f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;
    private PlayerStat _stat;
    private bool _isMine;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _stat = GetComponent<PlayerStat>();
    }
    
    private void Start()
    {
        PhotonView view = gameObject.GetComponent<PhotonView>();
        _isMine = view.IsMine;
        _yVelocity = 0f;
    }

    private void Update()
    {
        if (!_isMine) return;
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
        return transform.TransformDirection(movement) * _stat.MoveSpeed;
    }

    private void UpdateJump()
    {
        if (_characterController.isGrounded)
        {
            _yVelocity = Input.GetKey(KeyCode.Space) ? _stat.JumpForce : -1f;
        }
        _yVelocity -= GRAVITY * Time.deltaTime;
    }
}
