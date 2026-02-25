using Photon.Pun;
using UnityEngine;

public class ItemSpawner : SingletonBehaviour<ItemSpawner>
{
    [SerializeField] private GameObject _itemPrefab;
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
        SpawnItems(player.transform.position);
    }

    private void SpawnItems(Vector3 center)
    {
        int count = Random.Range(_minCount, _maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * _scatterRadius;
            Vector3 spawnPosition = center + new Vector3(randomCircle.x, _spawnHeight, randomCircle.y);
            PhotonNetwork.Instantiate(_itemPrefab.name, spawnPosition, Quaternion.identity);
        }
    }
}
