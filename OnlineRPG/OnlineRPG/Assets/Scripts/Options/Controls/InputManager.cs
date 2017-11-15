using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> keys;
    private Dictionary<string, IOptionsInfo> defaultKeybinds;

    List<KeyCode> bannedKeybindKeys;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one InputManager on the client!");
            return;
        }

        singleton = this;

        CheckFileDirectories();
        InitializeBannedKeys();
        SetDefaultKeys();
        LoadKeybinds();
    }

    void CheckFileDirectories()
    {
        DirectoryHelper.singleton.CheckDirectories();

        if (!File.Exists(Application.persistentDataPath + "/settings/controls.txt"))
        {
            File.CreateText(Application.persistentDataPath + "/settings/controls.txt");
        }
    }

    // Default KeyCodes for each input type in the dictionary.
    void SetDefaultKeys()
    {
        // Set Default Keys in case keybinds file is changed to illegal keys or corrupted by the user.
        defaultKeybinds = new Dictionary<string, IOptionsInfo>();

        defaultKeybinds["OPEN_INVENTORY"] = new KeybindInfo("", KeyCode.I);
        defaultKeybinds["OPEN_SKILLS"] = new KeybindInfo("", KeyCode.L);
        defaultKeybinds["OPEN_QUESTS"] = new KeybindInfo("", KeyCode.Q);
        defaultKeybinds["OPEN_CHAT"] = new KeybindInfo("", KeyCode.T);
        defaultKeybinds["OPEN_PAUSE_MENU"] = new KeybindInfo("", KeyCode.Escape);
        defaultKeybinds["TAKE_SCREENSHOT"] = new KeybindInfo("", KeyCode.F12);
    }

    // Loads the KeyCodes in from the file.
    void LoadKeybinds()
    {
        // By default everything is KeyCode.None.
        keys = new Dictionary<string, IOptionsInfo>();

        keys["OPEN_INVENTORY"] = new KeybindInfo("Inventory", KeyCode.None);
        keys["OPEN_SKILLS"] = new KeybindInfo("Skills", KeyCode.None);
        keys["OPEN_QUESTS"] = new KeybindInfo("Quest Book", KeyCode.None);
        keys["OPEN_CHAT"] = new KeybindInfo("Chat", KeyCode.None);
        keys["OPEN_PAUSE_MENU"] = new KeybindInfo("Pause Menu", KeyCode.None);
        keys["TAKE_SCREENSHOT"] = new KeybindInfo("Screenshot", KeyCode.None);

        CheckFileDirectories();

        OptionsHandler.LoadFromSettings(keys, defaultKeybinds, Application.persistentDataPath + "/settings/controls.txt");
    }

    void InitializeBannedKeys()
    {
        bannedKeybindKeys = new List<KeyCode>();
        bannedKeybindKeys.Add(KeyCode.LeftApple); // Left Windows Key.
        bannedKeybindKeys.Add(KeyCode.RightApple); // Right Windows key.
        bannedKeybindKeys.Add(KeyCode.Menu); // Menu Key (to the right of the Right Windows Key).
    }

    public void ResetKeybinds()
    {
        OptionsHandler.ResetSettings(keys, defaultKeybinds);

        WriteToFile();

        if (EventHandler.OnKeybindsChanged != null)
        {
            EventHandler.OnKeybindsChanged.Invoke();
        }
    }

    public List<KeyCode> GetBannedKeys()
    {
        return bannedKeybindKeys;
    }

    public KeybindInfo GetKey(string keyName)
    {
        if (keys.ContainsKey(keyName))
        {
            return (KeybindInfo)keys[keyName];
        }

        return new KeybindInfo("", KeyCode.None);
    }

    public List<string> GetAllKeyTypes()
    {
        return keys.Keys.ToList();
    }

    [Obsolete("You can now access the properly formatted name from the \"Name\" Property in KeybindInfo")]
    public string FormatKeyName(string keyName)
    {
        string[] words = keyName.Split('_');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].ToLower().ToUpperFirstChar();
        }

        return string.Join(" ", words);
    }

    public void SetKey(string keyName, object value)
    {
        OptionsHandler.SetSetting(keys, keyName, value);
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/controls.txt")) CheckFileDirectories();

        OptionsHandler.WriteToFile(keys, Application.persistentDataPath + "/settings/controls.txt");
    }
}