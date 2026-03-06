using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomManager : SingletonPunCallbacks<PhotonRoomManager>
{
    private Room _room;
    public Room Room => _room;
    public event Action OnDataChanged;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDied;
    public event Action<Dictionary<string, RoomInfo>> OnRoomListChanged;

    private readonly Dictionary<string, RoomInfo> _roomInfos = new Dictionary<string, RoomInfo>();

    protected override bool IsPersistent => true;

    public void CreateRoom(RoomCreationInfo info)
    {
        PhotonNetwork.NickName = info.Nickname;

        var roomOptions = new RoomOptions
        {
            MaxPlayers = info.MaxPlayers,
            IsVisible = info.IsVisible,
            IsOpen = info.IsOpen
        };

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "mn", info.Nickname }
        };

        roomOptions.CustomRoomPropertiesForLobby = new[]
        {
            "mn"
        };

        PhotonNetwork.CreateRoom(info.RoomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                _roomInfos.Remove(roomInfo.Name);
            }
            else
            {
                _roomInfos[roomInfo.Name] = roomInfo;
            }
        }

        OnRoomListChanged?.Invoke(_roomInfos);
    }

    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;

        PhotonNetwork.LoadLevel("GameScene");

        OnDataChanged?.Invoke();
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
        if (_room == null) return;
        if (!_room.Players.TryGetValue(attackerActorNumber, out Player attacker)) return;
        if (!_room.Players.TryGetValue(victimActorNumber, out Player victim)) return;
        OnPlayerDied?.Invoke(attacker.NickName, victim.NickName);
    }
}
