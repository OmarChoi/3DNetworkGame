using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

public class UI_RoomList : SingletonPunCallbacks<UI_RoomList>
{
    private List<UI_RoomItem> _roomItems = new List<UI_RoomItem>();
    private readonly Dictionary<string, RoomInfo> _roomInfos = new Dictionary<string, RoomInfo>();
    protected override void Init()
    {
        _roomItems = GetComponentsInChildren<UI_RoomItem>().ToList();
        HideAllRoomUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        HideAllRoomUI();

        foreach (var roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                _roomInfos.Remove(roomInfo.Name);
            }
            else
            {
                _roomInfos[roomInfo.Name] = roomInfo;
            }
        }

        int roomCount = _roomInfos.Count;
        List<RoomInfo> rooms = _roomInfos.Values.ToList();
        
        for (int i = 0; i < roomCount; i++)
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
