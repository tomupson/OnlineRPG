using UnityEngine;

public class EventHandler : MonoBehaviour
{
    #region Delegates
    public delegate void OnEnemyDeathDelegate(IEnemy enemy);
    public delegate void OnInventoryItemAddedDelegate(Item itemAdded);
    public delegate void OnInventoryItemRemovedDelegate(Item itemRemoved);
    public delegate void OnPlayerJoinedRoomDelegate(PhotonPlayer player);
    #endregion

    public static OnEnemyDeathDelegate OnEnemyDeath;
    public static OnInventoryItemAddedDelegate OnInventoryItemAdded;
    public static OnInventoryItemRemovedDelegate OnInventoryItemRemoved;
    public static OnPlayerJoinedRoomDelegate OnPlayerJoinedRoom;
}