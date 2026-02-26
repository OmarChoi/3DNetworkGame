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

    public void RequestDelete(int viewId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Delete(viewId);
        }
        else
        {
            _photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewId);
        }
    }
    
    [PunRPC]
    private void Delete(int viewId)
    {
        GameObject objectToDelete = PhotonView.Find(viewId)?.gameObject;
        if (objectToDelete == null) return;
        PhotonNetwork.Destroy(objectToDelete);
    }
    
    [PunRPC]
    private void MakeScoreItems(Vector3 makePosition)
    {
        int count = Random.Range(_minCount, _maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * _scatterRadius;

            // 소유자가 나가면 해당 네트워크 게임 오브젝트도 삭제된다.
            // 즉, 플레이어가 룸을 나가면 그 플레이어가 생성/소유한 모든 네트워크 게임 오브젝트는 삭제된다.
            // Instantiate로 생성한 Object는 Player 생명 주기를 가지고 있다.
            Vector3 spawnPosition = makePosition + new Vector3(randomCircle.x, _spawnHeight, randomCircle.y);
            PhotonNetwork.InstantiateRoomObject(_itemPrefab.name, spawnPosition, Quaternion.identity);

            // 포톤에는 룸 안에 방장(Master Client)이 있다.
            // 방을 만든 사람이 방장
            // - 방장을 양도할 수 있다.
            // - 방장이 게임을 나가면 자동으로 그 다음으로 들어온 사람이 방장이 된다.
        }
    }
}
