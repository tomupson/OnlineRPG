using System.IO;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    List<Item> database = new List<Item>();

    void Start()
    {
        database = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(Application.dataPath + "/JSON/items.json"));

        foreach (Item item in database)
        {
            item.Init();
        }
    }

    public Item FetchItem(int id)
    {
        Item found = database.Where(x => x.Id == id).FirstOrDefault();

        if (found != null) return found;

        return new Item();
    }

    public Item FetchItem(string slug)
    {
        Item found = database.Where(x => x.Slug == slug).FirstOrDefault();

        if (found != null) return found;

        return new Item();
    }
}