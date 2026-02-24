using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    
    private string _version = "0.0.1";
    private string _nickname = "MyNickname";
    
    private void Start()
    {
        _nickname = $"_{UnityEngine.Random.Range(100, 999)}";
        
        PhotonNetwork.GameVersion = _version;
        PhotonNetwork.NickName = _nickname;

        PhotonNetwork.SendRate = 30;            // 얼마나 자주 데이터를 송수신할 것인가 (초당 N번)
        PhotonNetwork.SerializationRate = 30;   // 얼마나 자주 데이터를 직렬화 할 것인지. (송수신 준비)
        
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

    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장 완료!");

        Debug.Log($"룸: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 인원: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 룸에 입장한 플레이어 정보
        Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Player> player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
        }
        
        PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤방 입장에 실패했습니다: {returnCode} - {message}");
        
        
        // 랜덤 룸 입장에 실패하면 룸이 하나도 없는 것이니 룸을 만들자!
        
        // 룸 옵션 정의
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 20, // 룸 최대 접속자 수
            IsVisible = true, // 로비에서 룸을 보여줄 것인지
            IsOpen = true // 룸의 오픈여부
        };

        // 룸 만들기
        PhotonNetwork.CreateRoom("NewGame", roomOptions);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패했습니다: {returnCode} - {message}");
    }
}
