using TMPro;
using UnityEngine;

public class NetworkRoomUI : MonoBehaviour
{
    #region Delegates
    public delegate void OnJoinRoomClickedDelegate(RoomInfo roomInfo);
    #endregion

    private OnJoinRoomClickedDelegate OnRoomSelectedCallback;
    public RoomInfo roomInfo;

    [SerializeField] private TextMeshProUGUI roomNameText;

    public void Setup(RoomInfo roomInfo, OnJoinRoomClickedDelegate OnRoomSelectedCallback)
    {
        this.roomInfo = roomInfo;
        this.OnRoomSelectedCallback = OnRoomSelectedCallback;
        roomNameText.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
    }

    public void SelectRoom()
    {
        if (OnRoomSelectedCallback != null)
        {
            OnRoomSelectedCallback.Invoke(roomInfo);
        }
    }
}