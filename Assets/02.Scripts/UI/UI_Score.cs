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
        Refresh();
    }

    private void OnDestroy()
    {
        ScoreManager.OnDataChanged -= Refresh;
    }
    
    private void Refresh()
    {
        var scores = ScoreManager.Instance.Scores;
        var scoresDatas = scores.Values?
                                .OrderByDescending(x => x.Score)
                                .ToList();
        if (scoresDatas == null) return;
        
        // todo. 1등부터 3등까지 정렬
        // todo. 3명 있는지 적절하게 반복문
        int nDatas = scoresDatas.Count;
        for(int i = 0; i < _items.Count; i++)
        {
            if (i >= nDatas) break;
            _items[i].Set(scoresDatas[i]);
        }
    }
}
