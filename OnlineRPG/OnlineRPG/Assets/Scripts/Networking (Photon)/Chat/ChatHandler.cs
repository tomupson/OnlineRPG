using System.Linq;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;

public class ChatHandler : MonoBehaviour, IChatClientListener
{
    #region Singleton
    public static ChatHandler singleton;
    #endregion

    ChatClient chatClient;

    bool connected = false;
    bool subscribedToRoomChannels = false;
    string roomChannelName;

    List<ChatFriend> friendsList = new List<ChatFriend>();
    Dictionary<ChatChannel, List<ChatSenderMessage>> channelsAndMessages = new Dictionary<ChatChannel, List<ChatSenderMessage>>();

    void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;

        chatClient = new ChatClient(this);
        chatClient.ChatRegion = "EU";
    }

    void Start()
    {
        LoadFriendsList();
    }

    void LoadFriendsList()
    {
        string friendsString = PlayerPrefs.GetString("FriendList");

        if (friendsString.Length <= 0) return;

        string[] friends = friendsString.Split(';');

        foreach (string friend in friends)
        {
            friendsList.Add(new ChatFriend()
            {
                Name = friend,
                Status = 0,
                StatusMessage = string.Empty
            });
        }
    }

    public ChatChannel GetChannelByName(string name)
    {
        ChatChannel channel;
        chatClient.TryGetChannel(name, out channel);
        return channel;
    }

    public List<ChatChannel> GetChannels()
    {
        return chatClient.PublicChannels.Select(x => x.Value).ToList();
    }

    public List<ChatFriend> GetAllFriends()
    {
        return friendsList;
    }

    public List<ChatSenderMessage> GetMessagesFromChannel(string channelName)
    {
        ChatChannel channel = GetChannelByName(channelName);
        if (channel != null)
        {
            return channelsAndMessages[channel];
        }
        return null;
    }

    void Update()
    {
        if (chatClient == null) return;

        if (!subscribedToRoomChannels && Player.LocalPlayer != null && connected)
        {
            JoinRoomChannels();
        }

        chatClient.Service();
    }

    public void SendText(string text, string channel, bool isPrivateMessage = false)
    {
        if (string.IsNullOrEmpty(text)) return;

        if (isPrivateMessage)
        {
            // In this case, channel is the user.
            chatClient.SendPrivateMessage(channel, text);
        }
        else
        {
            chatClient.PublishMessage(channel, text);
        }
    }

    void JoinRoomChannels()
    {
        subscribedToRoomChannels = true;

        roomChannelName = "room " + PhotonNetwork.room.Name.GetHashCode();
        chatClient.Subscribe(new string[] { roomChannelName });
    }

    public void Connect()
    {
        ExitGames.Client.Photon.Chat.AuthenticationValues authenticationValues = new ExitGames.Client.Photon.Chat.AuthenticationValues()
        {
            UserId = PhotonNetwork.player.UserId
        };

        chatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "1.0", authenticationValues);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"{level}: {message}");
    }

    public void OnDisconnected()
    {
        //Debug.Log("Disconnected");
    }

    public void OnConnected()
    {
        //Debug.Log("Connected");
        connected = true;
        chatClient.Subscribe(new string[] { "global" });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
        if (EventHandler.OnChatClientConnected != null)
            EventHandler.OnChatClientConnected.Invoke();
    }

    public void OnChatStateChange(ChatState state)
    {
        if (EventHandler.OnChatStateChanged != null)
        {
            EventHandler.OnChatStateChanged.Invoke(state);
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        List<ChatSenderMessage> senderWithMessage = new List<ChatSenderMessage>();
        for (int i = 0; i < senders.Length; i++)
        {
            senderWithMessage.Add(new ChatSenderMessage(senders[i], messages[i]));
        }

        channelsAndMessages[GetChannelByName(channelName)].AddRange(senderWithMessage);

        if (EventHandler.OnChatMessagesReceived != null)
        {
            EventHandler.OnChatMessagesReceived.Invoke(channelName, senders, messages);
        }
    }

    string GetReadableChannelName(string channelName)
    {
        return channelName.ToUpperFirstChar();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("Private Message recevied");
        Debug.Log($"[{channelName}] {sender}: {message}");
    }

    public void SetOnlineStatus(int status, string roomName = "")
    {
        chatClient.SetOnlineStatus(status, roomName);
    }

    public void AddFriend(string friendName)
    {
        // If nothing was entered.
        if (string.IsNullOrEmpty(friendName)) return;

        // If the friends list already contains the friend you're trying to add.
        if (friendsList.Find(x => x.Name == friendName) != null) return;

        chatClient.AddFriends(new string[] { friendName });

        ChatFriend newFriend = new ChatFriend()
        {
            Name = friendName,
            Status = 0,
            StatusMessage = string.Empty
        };

        friendsList.Add(newFriend);

        PlayerPrefs.SetString("FriendList", string.Join(";", GetFriendListNames()));

        if (EventHandler.OnChatFriendAdded != null)
        {
            EventHandler.OnChatFriendAdded.Invoke(newFriend);
        }
    }

    public void RemoveFriend(string friendName)
    {
        if (string.IsNullOrEmpty(friendName)) return;

        if (friendsList.Find(x => x.Name == friendName) == null) return;

        chatClient.RemoveFriends(new string[] { friendName });
        ChatFriend friendRemoved = friendsList.Where(x => x.Name == friendName).FirstOrDefault();
        friendsList.Remove(friendRemoved);

        PlayerPrefs.SetString("FriendList", string.Join(";", GetFriendListNames()));

        if (EventHandler.OnChatFriendRemoved != null)
        {
            EventHandler.OnChatFriendRemoved.Invoke(friendRemoved);
        }
    }

    string[] GetFriendListNames()
    {
        return friendsList.Select(x => x.Name).ToArray();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            ChatChannel channel = GetChannelByName(channels[i]);
            if (results[i] && !channelsAndMessages.ContainsKey(channel))
            {
                //Debug.Log($"Successfully subscribed to {channels[i]}");
                channelsAndMessages.Add(channel, new List<ChatSenderMessage>());
                if (EventHandler.OnChatChannelSubscribed != null)
                {
                    EventHandler.OnChatChannelSubscribed.Invoke(channel);
                }
            }
            //else Debug.LogWarning($"Failed to subscribe to {channels[i]}");
        }

        ChatMessage[] messagesToRemove;
        while (channelsAndMessages.Count > Constants.MAX_BACKLOG_MESSAGES)
        {

        }


    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("On Unsubscribed");

        foreach (KeyValuePair<ChatChannel, List<ChatSenderMessage>> channel in channelsAndMessages)
        {
            ChatChannel chatChannel = GetChannelByName(channel.Key.Name);
            if (chatChannel == null)
            {
                ChatChannel channelToRemove = channelsAndMessages.Where(x => x.Key.Name == channel.Key.Name).FirstOrDefault().Key;
                channelsAndMessages.Remove(channelToRemove);

                if (EventHandler.OnChatChannelUnsubscribed != null)
                {
                    EventHandler.OnChatChannelUnsubscribed.Invoke(channelToRemove);
                }
            }
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        if (EventHandler.OnChatUserStatusUpdated != null)
        {
            EventHandler.OnChatUserStatusUpdated.Invoke(user, status, gotMessage, message);
        }
    }
}

public struct ChatSenderMessage
{
    public readonly string sender;
    public readonly object message;

    public ChatSenderMessage(string sender, object message)
    {
        this.sender = sender;
        this.message = message;
    }
}