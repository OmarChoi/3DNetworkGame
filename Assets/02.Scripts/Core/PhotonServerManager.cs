using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    private string _version = "0.0.1";
    private string _nickname = "MyNickname";
    private void Start()
    {
        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.NickName = _nickname;
        
        PhotonNetwork.AutomaticallySyncScene = true;
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
        Debug.Log("Region: " + PhotonNetwork.CloudRegion);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료!");
        Debug.Log(PhotonNetwork.InLobby);
        
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료!");
        Debug.Log($"{PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"{PhotonNetwork.CurrentRoom.PlayerCount}");
        
        Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (var player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName}: {player.Value.ActorNumber}");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤방 입장에 실패했습니다: {returnCode} - {message}");
        
        
        // 랜덤 룸 입장에 실패하면 룸이 하나도 없는 것이니 룸을 만들자!
        
        // 룸 옵션 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;    // 룸 최대 접속자 수
        roomOptions.IsVisible = true;   // 로비에서 룸을 보여줄 것인지
        roomOptions.IsOpen = true;      // 룸의 오픈여부
        
        // 룸 만들기
        PhotonNetwork.CreateRoom("NewGame", roomOptions);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패했습니다: {returnCode} - {message}");
    }
}
