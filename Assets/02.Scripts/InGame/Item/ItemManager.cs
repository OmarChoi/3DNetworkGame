using Photon.Pun;
using UnityEngine;

public class ItemManager : SingletonPunCallbacks<ItemManager>
{
    [SerializeField] private GameObject _itemPrefab;

    public void RequestCreate(Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CreateRPC(position);
        }
        else
        {
            photonView.RPC(nameof(CreateRPC), RpcTarget.MasterClient, position);
        }
    }

    public void RequestDelete(int viewId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DeleteRPC(viewId);
        }
        else
        {
            photonView.RPC(nameof(DeleteRPC), RpcTarget.MasterClient, viewId);
        }
    }

    [PunRPC]
    private void CreateRPC(Vector3 position)
    {
        PhotonNetwork.InstantiateRoomObject(_itemPrefab.name, position, Quaternion.identity);
    }

    [PunRPC]
    private void DeleteRPC(int viewId)
    {
        GameObject objectToDelete = PhotonView.Find(viewId)?.gameObject;
        if (objectToDelete == null) return;
        PhotonNetwork.Destroy(objectToDelete);
    }
}