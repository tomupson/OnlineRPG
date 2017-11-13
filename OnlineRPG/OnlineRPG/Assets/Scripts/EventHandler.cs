using UnityEngine;
using ExitGames.Client.Photon.Chat;

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
    public delegate void OnKeybindsChangedDelegate();
    public delegate void OnGraphicsSettingsChangedDelegate();
    public delegate void OnAudioSettingsChangedDelegate();
    public delegate void OnGameSettingsChangedDelegate();
    #endregion

    #region Enemy
    public static OnEnemyDeathDelegate OnEnemyDeath;
    #endregion

    #region Inventory
    public static OnInventoryItemAddedDelegate OnInventoryItemAdded;
    public static OnInventoryItemRemovedDelegate OnInventoryItemRemoved;
    #endregion

    #region Photon
    public static OnPlayerJoinedRoomDelegate OnPlayerJoinedRoom;
    #endregion

    #region PhotonChat
    public static OnChatClientConnectedDelegate OnChatClientConnected;
    public static OnChatMessagesReceivedDelegate OnChatMessagesReceived;
    public static OnChatFriendAddedDelegate OnChatFriendAdded;
    public static OnChatFriendRemovedDelegate OnChatFriendRemoved;
    public static OnChatStateChangedDelegate OnChatStateChanged;
    public static OnChatUserStatusUpdatedDelegate OnChatUserStatusUpdated;
    #endregion

    #region Options
    public static OnKeybindsChangedDelegate OnKeybindsChanged;
    public static OnGraphicsSettingsChangedDelegate OnGraphicsSettingsChanged;
    public static OnAudioSettingsChangedDelegate OnAudioSettingsChanged;
    public static OnGameSettingsChangedDelegate OnGameSettingsChanged;
    #endregion
}