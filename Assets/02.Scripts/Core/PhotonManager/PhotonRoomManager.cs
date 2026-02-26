using System;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomManager : SingletonPunCallbacks<PhotonRoomManager>
{
    private Room _room;
    public Room Room => _room;
    public event Action OnDataChanged;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDeathed;
    
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        OnDataChanged?.Invoke();
        CharacterSpawner.Instance.SpawnPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerEnter?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(otherPlayer);
    }

    public void OnPlayerDeath(int attackerActorNumber, int victimActorNumber)
    {
        string attackerNickname = _room.Players[attackerActorNumber].NickName;
        string victimNickname = _room.Players[victimActorNumber].NickName;
        OnPlayerDeathed?.Invoke(attackerNickname, victimNickname);
    }
}
