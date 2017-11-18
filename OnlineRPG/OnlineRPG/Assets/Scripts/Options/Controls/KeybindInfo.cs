using UnityEngine;

[System.Serializable]
public class KeybindInfo : IOptionsInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public KeyCode Key { get; set; }

    public KeybindInfo(string name, string description, KeyCode key)
    {
        this.Name = name;
        this.Description = description;
        this.Key = key;
    }
}