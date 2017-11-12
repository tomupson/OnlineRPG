public interface IChatUI
{
    void ShowNewMessages(string channelName, string[] senders, object[] messages);
    void SendChatMessage();
}