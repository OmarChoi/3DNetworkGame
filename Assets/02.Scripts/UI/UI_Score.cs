using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    private List<UI_ScoreItem> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<UI_ScoreItem>().ToList();
        
        ScoreManager.OnDataChanged += Refresh;
        Refresh(0);
    }

    private void OnDestroy()
    {
        ScoreManager.OnDataChanged -= Refresh;
    }
    
    private void Refresh(int actorNumber)
    {
        var scores = ScoreManager.Instance.Scores;
        if (scores == null) return;
        var scoresData = scores.Values
                                .OrderByDescending(x => x.Score)
                                .ToList();
        int nData = scoresData.Count;
        for(int i = 0; i < _items.Count; i++)
        {
            if (i >= nData) break;
            _items[i].Set(scoresData[i]);
        }
    }
}
