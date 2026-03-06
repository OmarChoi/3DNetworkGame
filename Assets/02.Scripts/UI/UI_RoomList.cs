using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class UI_RoomList : MonoBehaviour
{
    private List<UI_RoomItem> _roomItems = new List<UI_RoomItem>();

    private void Awake()
    {
        _roomItems = GetComponentsInChildren<UI_RoomItem>().ToList();
        HideAllRoomUI();
    }

    private void OnEnable()
    {
        PhotonLobbyManager.Instance.OnRoomListChanged += UpdateRoomList;
    }

    private void OnDisable()
    {
        if (PhotonLobbyManager.Instance != null)
        {
            PhotonLobbyManager.Instance.OnRoomListChanged -= UpdateRoomList;
        }
    }

    private void UpdateRoomList(Dictionary<string, RoomInfo> roomInfos)
    {
        HideAllRoomUI();

        var rooms = roomInfos.Values.ToList();
        for (var i = 0; i < rooms.Count; i++)
        {
            _roomItems[i].Init(rooms[i]);
            _roomItems[i].gameObject.SetActive(true);
        }
    }

    private void HideAllRoomUI()
    {
        foreach (UI_RoomItem roomItem in _roomItems)
        {
            roomItem.gameObject.SetActive(false);
        }
    }
}
