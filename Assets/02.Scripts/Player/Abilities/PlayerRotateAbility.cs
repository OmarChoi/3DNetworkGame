using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    [SerializeField] private Transform _cameraRoot;
    private float _my;
    private float _mx;

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;
        Cursor.lockState = CursorLockMode.Locked;
        
        CinemachineCamera vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = _cameraRoot.transform;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (!_owner.PhotonView.IsMine) return;
        _mx += Input.GetAxis("Mouse X") * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += Input.GetAxis("Mouse Y") * _owner.Stat.RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(0f, _mx, 0f);
        _cameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }
}
