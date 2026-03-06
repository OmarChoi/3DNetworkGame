using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobbyManager : SingletonPunCallbacks<PhotonLobbyManager>
{
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
        _roomInfos.Clear();
        PhotonNetwork.LoadLevel("GameScene");
    }

}
