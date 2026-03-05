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

        SceneManager.LoadScene("GameScene");
        
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
