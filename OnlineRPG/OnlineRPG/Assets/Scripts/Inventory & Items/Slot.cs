using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    [HideInInspector] public int id;

    private Inventory inventory;
    
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (droppedItem != null)
        {
            if (inventory.items[id].Id == -1)
            {
                inventory.items[droppedItem.slot] = new Item(); // null out the item.
                inventory.items[id] = droppedItem.item;
                droppedItem.slot = id;
            }
            else if (droppedItem.slot != id)
            {
                // Already an item in the slot. Swap them.
                Transform item = transform.GetChild(0);
                ItemData itemData = item.GetComponent<ItemData>();
                itemData.slot = droppedItem.slot;
                Transform slotTransform = inventory.slots[droppedItem.slot].transform;
                item.transform.SetParent(slotTransform);
                item.transform.position = slotTransform.position;

                droppedItem.slot = id;
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.position = transform.position;

                inventory.items[droppedItem.slot] = itemData.item;
                inventory.items[id] = droppedItem.item;
            }
        }
    }
}