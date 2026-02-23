using UnityEngine;

public class PlayerRotateAbility : MonoBehaviour
{
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private float _rotationSpeed = 100f;
    private float _my;
    private float _mx;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        _mx += Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
        _my -= Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;
        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, _mx, 0f);
        _cameraRoot.localRotation = Quaternion.Euler(_my, 0f, 0f);
    }
}
