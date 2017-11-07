using TMPro;
using Photon;
using UnityEngine;

public class PhotonNetworkManager : Photon.MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI connectedText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lobbyCamera;
    [SerializeField] private Transform spawnPoint;
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(Constants.BUILD_VERSION);
    }

    void Update()
    {
        connectedText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("Joined lobby.");
        PhotonNetwork.JoinOrCreateRoom("Lobby", null, null);
    }

    public virtual void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity, 0);
        lobbyCamera.SetActive(false);
    }
}