using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonRoomManager : SingletonPunCallbacks<PhotonRoomManager>
{
    private Room _room;
    public Room Room => _room;
    public event Action OnDataChanged;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDied;
    
    protected override bool IsPersistent => true;
    
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;

        PhotonNetwork.LoadLevel("GameScene");
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     // 방장이 아니면 아무 것도 하지 않아도 방장(Master)이 있는 Scene으로 자동으로 옮겨진다.
        //     // Player 별로 각자 자신만의 Scene에서 작업하고 싶으면 AutomaticallySyncScene을 설정한다.
        // }
        
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
