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
        DeleteRPC(viewId);
    }

    public void RequestDelete(ItemInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DeleteRPC(info.ViewId);
        }
        else
        {
            photonView.RPC(nameof(DeleteRPC), RpcTarget.MasterClient, info.ViewId);
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