using ExitGames.Client.Photon.Chat;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    #region Delegates
    public delegate void OnEnemyDeathDelegate(IEnemy enemy);
    public delegate void OnInventoryItemAddedDelegate(Item itemAdded);
    public delegate void OnInventoryItemRemovedDelegate(Item itemRemoved);
    public delegate void OnPlayerJoinedRoomDelegate(PhotonPlayer player);
    public delegate void OnChatClientConnectedDelegate();
    public delegate void OnChatMessagesReceivedDelegate(string channelName, string[] senders, object[] messages);
    public delegate void OnChatFriendAddedDelegate(ChatFriend friendAdded);
    public delegate void OnChatFriendRemovedDelegate(ChatFriend friendRemoved);
    public delegate void OnChatStateChangedDelegate(ChatState state);
    public delegate void OnChatUserStatusUpdatedDelegate(string user, int status, bool gotMessage, object message);
    public delegate void OnKeybindsResetDelegate();
    #endregion

    public static OnEnemyDeathDelegate OnEnemyDeath;
    public static OnInventoryItemAddedDelegate OnInventoryItemAdded;
    public static OnInventoryItemRemovedDelegate OnInventoryItemRemoved;
    public static OnPlayerJoinedRoomDelegate OnPlayerJoinedRoom;
    public static OnChatClientConnectedDelegate OnChatClientConnected;
    public static OnChatMessagesReceivedDelegate OnChatMessagesReceived;
    public static OnChatFriendAddedDelegate OnChatFriendAdded;
    public static OnChatFriendRemovedDelegate OnChatFriendRemoved;
    public static OnChatStateChangedDelegate OnChatStateChanged;
    public static OnChatUserStatusUpdatedDelegate OnChatUserStatusUpdated;
    public static OnKeybindsResetDelegate OnKeybindsReset;
}