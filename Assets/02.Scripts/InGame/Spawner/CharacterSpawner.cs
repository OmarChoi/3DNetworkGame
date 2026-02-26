using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterSpawner : SingletonBehaviour<CharacterSpawner>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _respawnTime = 5f;
    private readonly HashSet<PlayerController> _respawningPlayers = new HashSet<PlayerController>();
    
    public void SpawnPlayer()
    {
        Vector3 position = GetRandomSpawnPosition();
        PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);
    }

    public void StartRespawn(PlayerController player)
    {
        if (!_respawningPlayers.Add(player)) return;
        StartCoroutine(Respawn_Coroutine(player));
    }
    
    private void RespawnPlayer(PlayerController player)
    {
        Vector3 position = GetRandomSpawnPosition();
        _respawningPlayers.Remove(player);
        player.ResetPlayer(position);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int index = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[index].position;
    }

    private IEnumerator Respawn_Coroutine(PlayerController player)
    {
        yield return new WaitForSeconds(_respawnTime);

        if (player == null)
        {
            _respawningPlayers.Remove(player);
            yield break;
        }

        RespawnPlayer(player);
    }
}