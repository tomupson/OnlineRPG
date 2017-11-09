using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using System;

public class ChatHandler : MonoBehaviour, IChatClientListener
{
    #region Singleton
    public static ChatHandler singleton;
    #endregion

    ChatClient chatClient;

    bool connected = false;
    bool subscribedToRoomChannels = false;
    string roomChannelName;

    Queue<string> messageQueue = new Queue<string>();
    List<ChatFriend> friendsList = new List<ChatFriend>();

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
        Connect();
    }

    public ChatChannel GetChannelByName(string name)
    {
        return GetChannels().Where(x => x.Name == name).FirstOrDefault();
    }

    void Start()
    {
        string friendsString = PlayerPrefs.GetString("FriendList");
        string[] friends = friendsString.Split(';');
        foreach (string friend in friends)
        {
            friendsList.Add(new ChatFriend()
            {
                Name = friend,
                Status = 0,
                StatusMessage = ""
            });
        }

        foreach (string name in friendsList.Select(x => x.Name).ToArray())
        {
            Debug.Log(name);
        }
    }

    public List<ChatChannel> GetChannels()
    {
        return chatClient.PublicChannels.Select(x => x.Value).ToList();
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

        roomChannelName = "Room " + PhotonNetwork.room.Name.GetHashCode();
        chatClient.Subscribe(new string[] { roomChannelName });
    }

    public void Connect()
    {
        ExitGames.Client.Photon.Chat.AuthenticationValues authenticationValues = new ExitGames.Client.Photon.Chat.AuthenticationValues()
        {
            UserId = PhotonNetwork.player.UserId
        };

        chatClient.Connect(Constants.PHOTON_CHAT_APP_ID, "1.0", authenticationValues);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"{level}: {message}");
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnected");
    }

    public void OnConnected()
    {
        Debug.Log("Connected");
        connected = true;
        chatClient.Subscribe(new string[] { "global" });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat State Changed");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            string message = "[" + GetReadableChannelName(channelName) + "]" +
                senders[i] + ": " +
                messages[i];

            messageQueue.Enqueue(message);
        }

        while (messageQueue.Count > 10)
        {
            messageQueue.Dequeue();
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
        if (string.IsNullOrEmpty(friendName)) return;

        if (friendsList.Find(x => x.Name == friendName) != null) return;

        chatClient.AddFriends(new string[] { friendName });
        friendsList.Add(new ChatFriend()
        {
            Name = friendName,
            Status = 0,
            StatusMessage = ""
        });

        PlayerPrefs.SetString("FriendList", string.Join(";", GetFriendListNames()));
    }

    public void RemoveFriend(string friendName)
    {
        if (string.IsNullOrEmpty(friendName)) return;

        if (friendsList.Find(x => x.Name == friendName) == null) return;

        chatClient.RemoveFriends(new string[] { friendName });
        friendsList.Remove(friendsList.Where(x => x.Name == friendName).FirstOrDefault());

        PlayerPrefs.SetString("FriendList", string.Join(";", GetFriendListNames()));
    }

    string[] GetFriendListNames()
    {
        return friendsList.Select(x => x.Name).ToArray();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("On Subscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log($"{channels[i]}: {results[i]}");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("On Unsubscribed");
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("On Status Update");
    }
}