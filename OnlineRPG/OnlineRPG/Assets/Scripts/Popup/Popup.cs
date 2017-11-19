using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    PopupInfo popupInfo;

    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Transform buttonArea;
    [SerializeField] private GameObject popupButtonPrefab;

    public void Setup(PopupInfo popupInfo)
    {
        this.popupInfo = popupInfo;
        descriptionText.text = popupInfo.Description;

        foreach (PopupButtonInfo buttonInfo in popupInfo.Buttons)
        {
            GameObject buttonGO = Instantiate(popupButtonPrefab, buttonArea, false);
            Button button = buttonGO.GetComponent<Button>();
            button.GetComponentInChildren<TMP_Text>().text = buttonInfo.ButtonText;
            button.onClick.AddListener(delegate
            {
                buttonInfo.OnPopupClick.Invoke();
                RemovePopup();
            });
        }
    }

    void RemovePopup()
    {
        Destroy(gameObject);
    }
}