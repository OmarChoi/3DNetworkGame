using Photon.Pun;
using UnityEngine;

public class Item : MonoBehaviourPun
{
    [SerializeField] private int _scoreAmount = 100;
    [SerializeField] private float _destroyHeight = -10f;
    [SerializeField] private float _lifeTime = 30f;

    private bool _isPickedUp;
    private bool _isDestroyRequested;
    private float _spawnTime;

    private void Start()
    {
        _spawnTime = Time.time;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_isPickedUp || _isDestroyRequested) return;
        bool isBelowDestroyHeight = transform.position.y < _destroyHeight;
        bool isExpired = Time.time - _spawnTime >= _lifeTime;
        if (!isBelowDestroyHeight && !isExpired) return;

        _isDestroyRequested = true;
        ItemManager.Instance.RequestDelete(photonView.ViewID);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isPickedUp) return;
        if (!other.gameObject.TryGetComponent(out PlayerController player)) return;
        if (!player.PhotonView.IsMine) return;
        
        _isPickedUp = true;
        ItemInfo info = new ItemInfo(photonView.ViewID, player.PhotonView.Owner.ActorNumber, _scoreAmount);
        ItemManager.Instance.RequestDelete(info);
    }
}
