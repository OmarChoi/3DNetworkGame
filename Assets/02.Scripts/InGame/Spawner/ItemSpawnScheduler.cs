using UnityEngine;
using Photon.Pun;

public class ItemSpawnScheduler : SingletonPunCallbacks<ItemSpawnScheduler>
{
    [Header("Spawn Position")]
    [SerializeField] private float _mapWidth;
    [SerializeField] private float _mapHeight;
    [SerializeField] private float _spawnHeight;

    [Space(10)]
    [Header("Spawn Options")]
    [SerializeField] private int _minSpawnCount;
    [SerializeField] private int _maxSpawnCount;
    [SerializeField] private float _spawnCooldown;
    private float _spawnTimer;

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _spawnTimer -= Time.deltaTime;
        if (!(_spawnTimer <= 0)) return;

        _spawnTimer = _spawnCooldown;
        SpawnItems();
    }

    private void SpawnItems()
    {
        int spawnCount = Random.Range(_minSpawnCount, _maxSpawnCount);
        for (int i = 0; i < spawnCount; i++)
        {
            float x = Random.Range(-_mapWidth * 0.5f, _mapWidth * 0.5f);
            float z = Random.Range(-_mapHeight * 0.5f, _mapHeight * 0.5f);
            Vector3 spawnPosition = new Vector3(x, _spawnHeight, z);

            ItemManager.Instance.RequestCreate(spawnPosition);
        }
    }
}