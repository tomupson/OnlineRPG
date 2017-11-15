using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text notificationTitleText;
    [SerializeField] private TMP_Text notificationMessageText;

    private Animator animator;

    public void Setup(NotificationInfo notificationInfo)
    {
        animator = GetComponent<Animator>();
        notificationTitleText.text = notificationInfo.Title;
        notificationMessageText.text = notificationInfo.Message;
    }
}