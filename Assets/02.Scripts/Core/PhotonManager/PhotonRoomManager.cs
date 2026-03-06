using System;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomManager : SingletonPunCallbacks<PhotonRoomManager>
{
    private Room _room;
    public Room Room => _room;
    public event Action<Player> OnPlayerEnter;
    public event Action<Player> OnPlayerLeft;
    public event Action<string, string> OnPlayerDied;

    protected override bool IsPersistent => true;

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
    }

    public override void OnLeftRoom()
    {
        _room = null;
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnter?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
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
