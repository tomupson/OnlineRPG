using UnityEngine;
using System.Collections.Generic;

public class NotificationHandler : MonoBehaviour
{
    #region Singleton
    public static NotificationHandler singleton;
    #endregion

    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform notificationParent;

    private List<Notification> notifications = new List<Notification>();

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one Notification Handler on the client!");
            return;
        }

        singleton = this;
    }

    public void DisplayNotification(NotificationInfo notificationInfo)
    {
        GameObject notificationGO = Instantiate(notificationPrefab, notificationParent, false);
        Notification notification = notificationGO.GetComponent<Notification>();
        notification.Setup(notificationInfo);
    }
}