using UnityEngine;

[System.Serializable]
public class KeybindInfo
{
    public string Name { get; set; }
    public KeyCode Key { get; set; }

    public KeybindInfo(string name, KeyCode key)
    {
        this.Name = name;
        this.Key = key;
    }
}