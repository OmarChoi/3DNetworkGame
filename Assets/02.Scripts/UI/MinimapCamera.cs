using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 25f, 0f);
    [SerializeField] private bool _followTargetYaw;

    private void LateUpdate()
    {
        if (_target == null && !TryFindLocalPlayerTarget()) return;
        Vector3 targetPosition = _target.position + _offset;
        transform.position = targetPosition;

        if (_followTargetYaw)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, _target.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = targetRotation;
        }
    }

    private bool TryFindLocalPlayerTarget()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController player in players)
        {
            if (player == null || player.PhotonView == null) continue;
            if (!player.PhotonView.IsMine) continue;

            _target = player.transform;
            return true;
        }

        return false;
    }
}
