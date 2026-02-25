using Photon.Pun;
using UnityEngine;

public class CharacterSpawner : SingletonBehaviour<CharacterSpawner>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    public void SpawnPlayer()
    {
        Vector3 position = GetRandomSpawnPosition();
        PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int index = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[index].position;
    }
}