using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    #region Singleton
    public static NetworkPlayer singleton;
    #endregion

    public string Username { get; private set; }

    void Awake()
    {
        singleton = this;

        string username = PlayerPrefs.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            UserSetup.singleton.CreateUser();
        } else
        {
            PhotonNetwork.playerName = PlayerPrefs.GetString("Username");
            FindObjectOfType<PhotonNetworkManager>().Connect();
        }
    }
}