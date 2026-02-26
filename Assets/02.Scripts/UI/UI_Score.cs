using TMPro;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _localActorNumber = -1;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += OnScoreChanged;
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= OnScoreChanged;
        }
    }
    
    private void OnEnable()
    {
        PlayerController.OnLocalPlayerCreated += OnLocalPlayerCreated;
    }

    private void OnDisable()
    {
        PlayerController.OnLocalPlayerCreated -= OnLocalPlayerCreated;
    }

    private void OnLocalPlayerCreated(PlayerController player)
    {
        _localActorNumber = player.PhotonView.Owner.ActorNumber;
    }

    private void OnScoreChanged(int actorNumber, int score)
    {
        if (_localActorNumber != actorNumber) return;
        _scoreText.text = $"Score: {score}";
    }
}
