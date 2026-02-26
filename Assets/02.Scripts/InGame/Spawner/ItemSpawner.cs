using UnityEngine;
using Photon.Pun;

public class ItemSpawner : SingletonPunCallbacks<ItemSpawner>
{
    // Master에서 생성 후 BroadCast
    [SerializeField] private GameObject _itemPrefab;
    
    [Space(10)]
    [Header("Spawn Position")]
    [SerializeField] private float _mapWidth;
    [SerializeField] private float _mapHeight;
    [SerializeField] private float _spawnHeight;
    
    [Space(10)]
    [Header("Spawn Options")]
    [SerializeField] private int _spawnCount;
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
        for (int i = 0; i < _spawnCount; i++)
        {
            float x = Random.Range(-_mapWidth * 0.5f, _mapWidth * 0.5f);
            float z = Random.Range(-_mapHeight * 0.5f, _mapHeight * 0.5f);
            Vector3 spawnPosition = new Vector3(x, _spawnHeight, z);

            PhotonNetwork.InstantiateRoomObject(_itemPrefab.name, spawnPosition, Quaternion.identity);
        }
    }
}