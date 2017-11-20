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
        PhotonNetwork.playerName = PlayerPrefs.GetString("Username");
        FindObjectOfType<PhotonNetworkManager>().Connect();
    }
}