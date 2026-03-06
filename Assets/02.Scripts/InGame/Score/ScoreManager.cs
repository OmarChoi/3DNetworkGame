using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ScoreManager : SingletonPunCallbacks<ScoreManager>
{
    private const string Key = "score";
    private int _score;

    private readonly Dictionary<int, ScoreData> _scores = new Dictionary<int, ScoreData>();
    public ReadOnlyDictionary<int, ScoreData> Scores => new ReadOnlyDictionary<int, ScoreData>(_scores);
    public static event Action<int> OnDataChanged;
    protected override void Init()
    {
        PlayerController.OnLocalPlayerCreated += OnLocalPlayerCreated;
        PlayerController.OnPlayerDied += subtractHalf;
    }

    private void OnLocalPlayerCreated(PlayerController player)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            ScoreData scoreData = new ScoreData()
            {
                Nickname = p.NickName,
                Score = p.CustomProperties.TryGetValue(Key, out object value) ? (int)value : 0,
            };
            _scores[p.ActorNumber] = scoreData;
        }
        Refresh();
    }

    private void OnDestroy()
    {
        PlayerController.OnLocalPlayerCreated -= OnLocalPlayerCreated;
        PlayerController.OnPlayerDied -= subtractHalf;
    }

    public void AddScore(int score)
    {
        _score += score;
        Refresh();
    }

    private void subtractHalf(PlayerController player)
    {
        if (!player.PhotonView.IsMine) return;
        int subtractAmount = (int)(_score / 2.0f);
        SubtractScore(subtractAmount);
    }
    
    public void SubtractScore(int score)
    {
        _score = Mathf.Max(0, _score - score);
        Refresh();
    }
    
    private void Refresh()
    {
        var hashtable = new Hashtable();
        hashtable.Add(Key, _score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.TryGetValue(Key, out var value)) return;
        if (value is not int intValue) return;

        ScoreData scoreData = new ScoreData()
        {
            Nickname = targetPlayer.NickName,
            Score = intValue
        };
        
        _scores[targetPlayer.ActorNumber] = scoreData;
        OnDataChanged?.Invoke(targetPlayer.ActorNumber);
    }
}