using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon.Chat;

public class PhotonNetworkManager : Photon.MonoBehaviour
{
    [SerializeField] private TMP_Text connectionStatusText;
    [SerializeField] private GameObject player;

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        EventHandler.OnChatClientConnected += SetUserStatus;
        CustomTypeSerialization.Register();
    }

    void Start()
    {
        //Connect();
    }

    void Update()
    {
        if (connectionStatusText != null)
        {
            connectionStatusText.text = PhotonNetwork.connectionStateDetailed.ToString();
        }
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(Constants.BUILD_VERSION);
    }

    void SetUserStatus()
    {
        ChatHandler.singleton.SetOnlineStatus(ChatUserStatus.Online);
    }

    void OnConnectedToMaster()
    {
        //Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        ChatHandler.singleton.Connect();
    }

    void OnJoinedLobby()
    {
        //Debug.Log("Joined Lobby.");
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level")
        {
            // Spawn Pos is temporary until a database is made.
            PhotonNetwork.Instantiate(player.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
            ChatHandler.singleton.SetOnlineStatus(ChatUserStatus.Playing, PhotonNetwork.room.Name);
        }
    }
}