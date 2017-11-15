using UnityEngine;
using ExitGames.Client.Photon.Chat;

public class ChannelUI : MonoBehaviour
{
    public ChatChannel chatChannel;

    public void Setup(ChatChannel channel)
    {
        this.chatChannel = channel;
    }
}