using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _masterNicknameText;
    [SerializeField] private TextMeshProUGUI _playerCountText;
    [SerializeField] private Button _roomEnterButton;
    
    private RoomInfo _roomInfo;

    private void Start()
    {
        _roomEnterButton.onClick.AddListener(EnterRoom);
    }
    
    public void Init(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _roomNameText.text = roomInfo.Name;
        _masterNicknameText.text = roomInfo.CustomProperties.TryGetValue("mn", out var mn) ? mn.ToString() : "Unknown";
        _playerCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }

    private void EnterRoom()
    {
        if (_roomInfo == null) return;
        PhotonLobbyManager.Instance.JoinRoom(_roomInfo.Name);

    }
}
