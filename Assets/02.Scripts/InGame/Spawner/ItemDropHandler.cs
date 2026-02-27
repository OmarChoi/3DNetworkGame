using UnityEngine;

public class ItemDropHandler : MonoBehaviour
{
    [SerializeField] private float _spawnHeight = 1f;
    [SerializeField] private float _scatterRadius = 2f;
    [SerializeField] private int _minCount = 3;
    [SerializeField] private int _maxCount = 5;

    private void OnEnable()
    {
        PlayerController.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied(PlayerController player)
    {
        if (!player.PhotonView.IsMine) return;
        DropScoreItems(player.transform.position);
    }

    private void DropScoreItems(Vector3 basePosition)
    {
        int count = Random.Range(_minCount, _maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * _scatterRadius;
            Vector3 spawnPosition = basePosition + new Vector3(randomCircle.x, _spawnHeight, randomCircle.y);
            ItemManager.Instance.RequestCreate(spawnPosition);
        }
    }
}