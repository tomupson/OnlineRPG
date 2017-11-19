using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    #region Singleton
    public static PopupHandler singleton;
    #endregion

    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Transform content;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one Popup Handler on the client!");
            return;
        }

        singleton = this;
    }

    public void CreatePopup(PopupInfo popupInfo)
    {
        GameObject popupGO = Instantiate(popupPrefab, content, false);
        Popup popup = popupGO.GetComponent<Popup>();
        popup.Setup(popupInfo);
    }
}