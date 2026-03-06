public readonly struct RoomCreationInfo
{
    public string RoomName { get; }
    public int MaxPlayers { get; }
    public bool IsVisible { get; }
    public bool IsOpen { get; }

    public RoomCreationInfo(string roomName, int maxPlayers = 20, bool isVisible = true, bool isOpen = true)
    {
        RoomName = roomName;
        MaxPlayers = maxPlayers;
        IsVisible = isVisible;
        IsOpen = isOpen;
    }
}
