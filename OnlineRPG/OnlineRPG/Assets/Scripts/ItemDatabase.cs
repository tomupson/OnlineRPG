using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();

    void Start()
    {
        database = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(Application.dataPath + "/items.json"));
        foreach (Item item in database)
        {
            item.GetSprite();
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

[System.Serializable]
public sealed class Item
{
    public int Id { get; set; } = -1;
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public int MaxStack { get; set; }
    public List<string> Uses { get; set; }

    public Item() { }

    public void GetSprite() => Sprite = Resources.Load<Sprite>(string.Format("Items/{0}", Slug));
}