using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button joinRoomButton;

    [HideInInspector] public ChatFriend friend;

    void Update()
    {
        statusText.text = string.Format("{0} {1}", ChatHelper.GetHumanReadableStatus(friend.Status), !string.IsNullOrEmpty(friend.StatusMessage) ? " (" + friend.StatusMessage + ")" : "");
    }

    public void Setup(ChatFriend friend)
    {
        joinRoomButton.gameObject.SetActive(false);
        this.friend = friend;
        friendNameText.text = friend.Name;
        UpdateStatusText();
    }

    public void RemoveFriend()
    {
        ChatHandler.singleton.RemoveFriend(friend.Name);
    }

    public void JoinFriendsGame()
    {
        PhotonNetwork.JoinRoom(friend.Room.Name);
    }

    public void UpdateStatus(int status, bool gotMessage, object message)
    {
        friend.Status = status;

        if (gotMessage)
        {
            friend.StatusMessage = message.ToString();
            friend.Room = PhotonNetwork.GetRoomList().Where(x => x.Name == message.ToString()).FirstOrDefault();
            joinRoomButton.gameObject.SetActive(true);
        }
        else
        {
            friend.StatusMessage = string.Empty;
            friend.Room = null;
            joinRoomButton.gameObject.SetActive(false);
        }

        UpdateStatusText();
    }

    void UpdateStatusText()
    {
        statusText.text = string.Format("{0} {1}", ChatHelper.GetHumanReadableStatus(friend.Status), !string.IsNullOrEmpty(friend.StatusMessage) ? " (" + friend.StatusMessage + ")" : "");
    }
}