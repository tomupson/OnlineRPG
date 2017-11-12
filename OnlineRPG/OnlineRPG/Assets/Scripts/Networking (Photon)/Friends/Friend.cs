using TMPro;
using UnityEngine;

public class Friend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    [SerializeField] private TMP_Text statusText;

    [HideInInspector] public ChatFriend friend;

    public void Setup(ChatFriend friend)
    {
        this.friend = friend;
        friendNameText.text = friend.Name;
        statusText.text = string.Format("{0} {1}", ChatHelper.GetHumanReadableStatus(friend.Status), !string.IsNullOrEmpty(friend.StatusMessage) ? " (" + friend.StatusMessage + ")" : "");
    }

    public void RemoveFriend()
    {
        ChatHandler.singleton.RemoveFriend(friend.Name);
    }

    void Update()
    {
        statusText.text = string.Format("{0} {1}", ChatHelper.GetHumanReadableStatus(friend.Status), !string.IsNullOrEmpty(friend.StatusMessage) ? " (" + friend.StatusMessage + ")" : "");
    }
}