using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkLobby : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject roomUIPrefab;
    [SerializeField] private Color selectedRoomImageColor;

    private List<NetworkRoomUI> roomList = new List<NetworkRoomUI>();
    RoomInfo selectedRoom;

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("Update Room List");
        ClearRoomList();

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomUIGO = Instantiate(roomUIPrefab, content, false);
                NetworkRoomUI roomUI = roomUIGO.GetComponent<NetworkRoomUI>();
                roomUI.Setup(room, OnRoomSelected);
                roomList.Add(roomUI);
            }
        }

        CheckSelectedRoom();
    }

    void PickRandomRoom()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        if (rooms.Length == 0) return;
        selectedRoom = rooms[new System.Random().Next(0, rooms.Length)];

        if (selectedRoom.IsOpen && selectedRoom.PlayerCount < selectedRoom.MaxPlayers)
        {
            NetworkRoomUI roomUI = roomList.Where(x => x.roomInfo == selectedRoom).FirstOrDefault();
            if (roomUI != null)
            {
                roomUI.GetComponent<Image>().color = selectedRoomImageColor;
            }
            else PickRandomRoom();
        }
        else PickRandomRoom();
    }

    void CheckSelectedRoom()
    {
        if (!(roomList.Any(x => x.roomInfo == selectedRoom)))
        {
            // Room has gone (or hasn't been selected), pick new selected room.
            PickRandomRoom();
        }

        return;
    }

    void ClearRoomList()
    {
        foreach (NetworkRoomUI roomListItem in roomList)
        {
            Destroy(roomListItem.gameObject);
        }

        roomList.Clear();
    }

    void OnRoomSelected(RoomInfo room) => selectedRoom = room;

    public void JoinRoom()
    {
        if (selectedRoom != null)
        {
            PhotonNetwork.JoinRoom(selectedRoom.Name);
        }
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 20 };
        if (PhotonNetwork.CreateRoom("EU", roomOptions, TypedLobby.Default))
        {
            Debug.Log("CreateRoom request sent successfully.");
        } else
        {
            Debug.LogWarning("Failed to send CreateRoom request.");
        }
    }

    void OnCreatedRoom()
    {
        Debug.Log("Room Created Successfully!");
    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        PhotonNetwork.LoadLevel("Level");
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        Debug.Log("Create Room Failed: " + codeAndMessage[1]);
    }
}