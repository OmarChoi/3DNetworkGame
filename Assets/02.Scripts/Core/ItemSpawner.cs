using Photon.Pun;
using UnityEngine;

public class ItemSpawner : SingletonBehaviour<ItemSpawner>
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _spawnHeight = 1f;
    [SerializeField] private float _scatterRadius = 2f;
    [SerializeField] private int _minCount = 3;
    [SerializeField] private int _maxCount = 5;
    private PhotonView _photonView;

    private void OnEnable()
    {
        PlayerController.OnPlayerDied += OnPlayerDied;
        _photonView = GetComponent<PhotonView>();
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied(PlayerController player)
    {
        if (!player.PhotonView.IsMine) return;
        RequestMakeScoreItems(player.transform.position);
    }

    private void RequestMakeScoreItems(Vector3 makePosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MakeScoreItems(makePosition);
        }
        else
        {
            _photonView.RPC(nameof(MakeScoreItems), RpcTarget.MasterClient, makePosition);
        }
    }

    [PunRPC]
    private void MakeScoreItems(Vector3 makePosition)
    {
        int count = Random.Range(_minCount, _maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * _scatterRadius;
            Vector3 spawnPosition = makePosition + new Vector3(randomCircle.x, _spawnHeight, randomCircle.y);
            PhotonNetwork.InstantiateRoomObject(_itemPrefab.name, spawnPosition, Quaternion.identity);
        }
    }
}
