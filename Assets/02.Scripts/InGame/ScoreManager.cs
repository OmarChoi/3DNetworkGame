using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : SingletonPunCallbacks<ScoreManager>
{
    private readonly Dictionary<int, int> _scores = new Dictionary<int, int>();

    public event Action<int, int> OnScoreChanged; // actorNumber, newScore

    public int GetScore(int actorNumber)
    {
        return _scores.TryGetValue(actorNumber, out int score) ? score : 0;
    }

    public Dictionary<int, int> GetAllScores()
    {
        return new Dictionary<int, int>(_scores);
    }

    public void RequestAddScore(int actorNumber, int amount)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber) return;
        if (PhotonNetwork.IsMasterClient)
        {
            AddScoreRPC(actorNumber, amount);
        }
        else
        {
            photonView.RPC(nameof(AddScoreRPC), RpcTarget.MasterClient, actorNumber, amount);
        }
    }

    [PunRPC]
    private void AddScoreRPC(int actorNumber, int amount)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _scores.TryGetValue(actorNumber, out int current);
        int newScore = current + amount;
        _scores[actorNumber] = newScore;

        photonView.RPC(nameof(UpdateScoreRPC), RpcTarget.All, actorNumber, newScore);
    }

    [PunRPC]
    private void UpdateScoreRPC(int actorNumber, int newScore)
    {
        _scores[actorNumber] = newScore;
        OnScoreChanged?.Invoke(actorNumber, newScore);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _scores.Remove(otherPlayer.ActorNumber);
    }
}