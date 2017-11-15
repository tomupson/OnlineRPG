[System.Serializable]
public class ChatFriend
{
    public string Name { get; set; }
    public int Status { get; set; }
    public string StatusMessage { get; set; }
    public RoomInfo Room { get; set; }
}