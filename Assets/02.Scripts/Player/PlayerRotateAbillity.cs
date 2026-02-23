using UnityEngine;

public class PlayerRotateAbillity : MonoBehaviour
{
    public Transform CameraRoot;
    public float RotationSpeed = 100f;
    private float _my;
    private float _mx;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        _mx += Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
        _my -= Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime;
        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, _mx, 0f);
        CameraRoot.localRotation = Quaternion.Euler(_my, 0f, 0f);
    }
}
