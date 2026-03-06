using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobbyManager : SingletonPunCallbacks<PhotonLobbyManager>
{
    public event Action<Dictionary<string, RoomInfo>> OnRoomListChanged;

    private readonly Dictionary<string, RoomInfo> _roomInfos = new Dictionary<string, RoomInfo>();

    protected override bool IsPersistent => true;

    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    private void EnsureNickname()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            PhotonNetwork.NickName = $"guest_{UnityEngine.Random.Range(100, 999)}";
        }
    }

    public void CreateRoom(RoomCreationInfo info)
    {
        EnsureNickname();

        var roomOptions = new RoomOptions
        {
            MaxPlayers = info.MaxPlayers,
            IsVisible = info.IsVisible,
            IsOpen = info.IsOpen
        };

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { PhotonPropertyKeys.MasterNickname, PhotonNetwork.NickName }
        };

        roomOptions.CustomRoomPropertiesForLobby = new[]
        {
            PhotonPropertyKeys.MasterNickname
        };

        PhotonNetwork.CreateRoom(info.RoomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        EnsureNickname();
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
