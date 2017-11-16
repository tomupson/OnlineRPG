using TMPro;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class FriendsList : MonoBehaviour
{
    private List<Friend> friendList = new List<Friend>();

    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private TMP_InputField addFriendInputField;

    void Start()
    {
        EventHandler.OnChatClientConnected += SetupFriendsList;
        EventHandler.OnChatFriendAdded += ChatFriendAdded;
        EventHandler.OnChatFriendRemoved += ChatFriendRemoved;
        EventHandler.OnChatUserStatusUpdated += ChatFriendStatusChanged;
    }

    void SetupFriendsList()
    {
        List<ChatFriend> friends = ChatHandler.singleton.GetAllFriends();
        foreach (ChatFriend friend in friends)
        {
            CreateFriendGO(friend);
        }
    }

    void CreateFriendGO(ChatFriend friend)
    {
        GameObject friendGO = Instantiate(friendPrefab, content, false);
        Friend friendComponent = friendGO.GetComponent<Friend>();
        friendComponent.Setup(friend);
        friendList.Add(friendComponent);
    }

    void RemoveFriendGO(ChatFriend friend)
    {
        Destroy(friendList.Find(x => x.friend == friend).gameObject);
    }

    void ChatFriendAdded(ChatFriend newFriend)
    {
        CreateFriendGO(newFriend);
    }

    void ChatFriendRemoved(ChatFriend friendRemoved)
    {
        RemoveFriendGO(friendRemoved);
    }

    void ChatFriendStatusChanged(string user, int status, bool gotMessage, object message)
    {
        Friend friendFound = friendList.Where(x => x.friend.Name == user).FirstOrDefault();
        if (friendFound != null)
        {
            friendFound.UpdateStatus(status, gotMessage, message);
        }
    }

    public void AddFriend()
    {
        if (string.IsNullOrEmpty(addFriendInputField.text)) return;

        ChatHandler.singleton.AddFriend(addFriendInputField.text);
        addFriendInputField.Clear();
    }
}