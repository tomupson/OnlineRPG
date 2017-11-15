using ExitGames.Client.Photon.Chat;
using UnityEngine;

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

    public static string GetHumanReadableChannelName(string channelName)
    {
        if (PhotonNetwork.room != null)
        {
            if (channelName == "room " + PhotonNetwork.room.Name.GetHashCode())
                return "Room";
        }

        string[] words = channelName.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].ToUpperFirstChar();
        }

        return string.Join(" ", words);
    }
}