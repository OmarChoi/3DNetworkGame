using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonServerManager : SingletonPunCallbacks<PhotonServerManager>
{
    private string _version = "0.0.1";
    private string _nickname = "MyNickname";
    protected override bool IsPersistent => true;
    
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
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤방 입장에 실패했습니다: {returnCode} - {message}");
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장에 실패했습니다: {returnCode} - {message}");
    }
}
