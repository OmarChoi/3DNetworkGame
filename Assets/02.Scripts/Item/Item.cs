using Photon.Pun;
using UnityEngine;

public class Item : MonoBehaviourPun
{
    [SerializeField] private int _scoreAmount = 100;
    private bool _isPickedUp;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isPickedUp) return;
        if (!other.gameObject.TryGetComponent(out PlayerController player)) return;
        if (!player.PhotonView.IsMine) return;
        
        _isPickedUp = true;
        ItemSpawner.Instance.RequestDelete(photonView.ViewID);
    }
}
