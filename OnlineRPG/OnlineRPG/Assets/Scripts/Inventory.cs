using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private int maxSlots;

    ItemDatabase database;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        database = GetComponent<ItemDatabase>();
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, content.transform);
            slots.Add(newSlot);
            slots[i].GetComponent<Slot>().id = i;
            items.Add(new Item());
        }
    }

    public void AddItem(Item itemToAdd)
    {
        int slotId = GetSlotFromItem(itemToAdd);
        if (itemToAdd.Stackable && slotId != -1)
        {
            ItemData data = slots[slotId].transform.GetChild(0).GetComponent<ItemData>();
            data.amount++;
            data.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.amount.ToString();
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == -1) // Empty slot found.
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItemPrefab, slots[i].transform);
                    ItemData itemData = itemObj.GetComponent<ItemData>();
                    itemData.item = itemToAdd;
                    itemData.slot = i;
                    itemData.amount = 1;
                    itemData.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // By default it shows nothing.
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Slug;
                    break;
                }
            }
        }
    }

    public void AddItem(int id)
    {
        AddItem(database.FetchItem(id));
    }

    public void AddItem(string slug)
    {
        if (string.IsNullOrEmpty(slug)) return;
        AddItem(database.FetchItem(slug));
    }

    public void RemoveItem(Item itemToRemove, int amountToRemove)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == itemToRemove.Id)
            {
                ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                if (amountToRemove >= data.amount)
                {

                } else
                {
                    data.amount -= amountToRemove;
                }
            }
        }
    }

    public void RemoveItem(string slug, int amount)
    {
        if (string.IsNullOrEmpty(slug)) return;
        RemoveItem(database.FetchItem(slug), amount);
    }

    public void RemoveItem(int id, int amount)
    {
        RemoveItem(database.FetchItem(id), amount);
    }

    int GetSlotFromItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == item.Id) return i;
        }

        return -1;
    }
}