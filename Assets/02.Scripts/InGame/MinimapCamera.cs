using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = new Vector3(0f, 25f, 0f);
    [SerializeField] private bool _followTargetYaw;

    private Transform _target;

    private void OnEnable()
    {
        PlayerController.OnLocalPlayerCreated += OnLocalPlayerCreated;
    }

    private void OnDisable()
    {
        PlayerController.OnLocalPlayerCreated -= OnLocalPlayerCreated;
    }

    private void OnLocalPlayerCreated(PlayerController player)
    {
        _target = player.transform;
    }

    private void LateUpdate()
    {
        if (_target == null) return;
        Vector3 targetPosition = _target.position + _offset;
        transform.position = targetPosition;

        if (_followTargetYaw)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, _target.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = targetRotation;
        }
    }
}
