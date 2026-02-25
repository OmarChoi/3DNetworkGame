using System;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance { get; private set; }
    private Room _room;
    public Room Room => _room;
    public event Action OnDataChanged;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        OnDataChanged?.Invoke();
        CharacterSpawner.Instance.SpawnPlayer();
    }
}
