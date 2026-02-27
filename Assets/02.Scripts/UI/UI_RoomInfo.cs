using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _playerCountText;
    [SerializeField] private Button _roomExitButton;

    private void Start()
    {
        _roomExitButton.onClick.AddListener(ExitRoom);
        PhotonRoomManager.Instance.OnDataChanged += Refresh;
        Refresh();
    }
    
    private void OnDestroy()
    {
        if (PhotonRoomManager.Instance != null)
        {
            PhotonRoomManager.Instance.OnDataChanged -= Refresh;
        }
    }
    
    private void ExitRoom()
    {
        // todo. 구현 예정
    }
    
    private void Refresh()
    {
        Room room = PhotonRoomManager.Instance.Room;
        if (room == null) return;
        _roomNameText.text = room.Name;
        _playerCountText.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }
}
