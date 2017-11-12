using ExitGames.Client.Photon.Chat;
public static class ChatHelper
{
    public static string GetHumanReadableStatus(int status)
    {
        switch (status)
        {
            case ChatUserStatus.Away:
                return "Away";
            case ChatUserStatus.DND:
                return "Do Not Disturb";
            case ChatUserStatus.Invisible:
                return "Invisible";
            case ChatUserStatus.LFG:
                return "Looking for a game"; // Probably not going to ever be implemented in this game.
            default:
            case ChatUserStatus.Offline:
                return "Offline";
            case ChatUserStatus.Online:
                return "Online";
            case ChatUserStatus.Playing:
                return "Playing";
        }
    }
}