public readonly struct RoomCreationInfo
{
    public string RoomName { get; }
    public string Nickname { get; }
    public int MaxPlayers { get; }
    public bool IsVisible { get; }
    public bool IsOpen { get; }

    public RoomCreationInfo(string roomName, string nickname, int maxPlayers = 20, bool isVisible = true, bool isOpen = true)
    {
        RoomName = roomName;
        Nickname = nickname;
        MaxPlayers = maxPlayers;
        IsVisible = isVisible;
        IsOpen = isOpen;
    }
}
