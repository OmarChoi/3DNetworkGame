using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_ScoreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nicknameText;

    private void Awake()
    {
        _nicknameText.richText = false;
    }
    
    public void Set(ScoreData scoreData)
    {
        _nicknameText.text = scoreData.Nickname;
        _scoreText.SetText("{0}", scoreData.Score);
    }
}