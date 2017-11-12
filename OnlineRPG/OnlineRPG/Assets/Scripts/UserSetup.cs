using TMPro;
using UnityEngine;

public class UserSetup : MonoBehaviour
{
    #region Singleton
    public static UserSetup singleton;
    #endregion

    [SerializeField] private GameObject maskPanel;
    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private TMP_InputField usernameInputField;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Multiple UserSetups on the client!");
            return;
        }

        singleton = this;
    }

    public void CreateUser()
    {
        maskPanel.SetActive(true);
        usernamePanel.SetActive(true);
    }

    public void SetUsername()
    {
        if (string.IsNullOrEmpty(usernameInputField.text)) return;

        maskPanel.SetActive(false);
        usernamePanel.SetActive(false);
        string username = usernameInputField.text + "#" + Random.Range(0, 10000);
        PlayerPrefs.SetString("Username", username);
        PhotonNetwork.playerName = username;
        FindObjectOfType<PhotonNetworkManager>().Connect();
    }
}