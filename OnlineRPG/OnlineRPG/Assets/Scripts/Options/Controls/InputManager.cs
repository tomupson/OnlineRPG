using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour, IOptionHandler
{
    #region Singleton
    public static InputManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> keys;
    private Dictionary<string, IOptionsInfo> defaultKeybinds;

    Dictionary<string, object> valuesToBeChanged = new Dictionary<string, object>();

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

        defaultKeybinds["OPEN_INVENTORY"] = new KeybindInfo(string.Empty, string.Empty, KeyCode.I);
        defaultKeybinds["OPEN_SKILLS"] = new KeybindInfo(string.Empty, string.Empty, KeyCode.L);
        defaultKeybinds["OPEN_QUESTS"] = new KeybindInfo(string.Empty, string.Empty, KeyCode.Q);
        defaultKeybinds["OPEN_CHAT"] = new KeybindInfo(string.Empty, string.Empty, KeyCode.T);
        defaultKeybinds["OPEN_PAUSE_MENU"] = new KeybindInfo(string.Empty, string.Empty, KeyCode.Escape);
        defaultKeybinds["TAKE_SCREENSHOT"] = new KeybindInfo(string.Empty, string.Empty, KeyCode.F12);
    }

    // Loads the KeyCodes in from the file.
    void LoadKeybinds()
    {
        // By default everything is KeyCode.None.
        keys = new Dictionary<string, IOptionsInfo>();

        keys["OPEN_INVENTORY"] = new KeybindInfo("Inventory", "The Inventory holds your items.", KeyCode.None);
        keys["OPEN_SKILLS"] = new KeybindInfo("Skills", "Skills allow you to obtain items faster by engaging in common activities.", KeyCode.None);
        keys["OPEN_QUESTS"] = new KeybindInfo("Quest Book", "Quests allow you to gain items and experience by completing goals.", KeyCode.None);
        keys["OPEN_CHAT"] = new KeybindInfo("Chat", "Chat allows you to talk to other players in many channels.", KeyCode.None);
        keys["OPEN_PAUSE_MENU"] = new KeybindInfo("Pause Menu", "Displays options.", KeyCode.None);
        keys["TAKE_SCREENSHOT"] = new KeybindInfo("Screenshot", "Takes a screenshot of the game.", KeyCode.None);

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
        PopupHandler.singleton.CreatePopup(new PopupInfo("Are you sure you want to reset your keybinds?", new List<PopupButtonInfo>()
        {
            new PopupButtonInfo("Yes", ConfirmResetKeybinds),
            new PopupButtonInfo("No", delegate { Debug.Log("Cancelled"); })
        }));
    }

    public void ConfirmResetKeybinds()
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

        return new KeybindInfo(string.Empty, string.Empty, KeyCode.None);
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
        bool isOriginal = OptionsHelper.CheckIfSettingIsOriginal(keys, keyName, value);

        if (isOriginal) return;
        else
        {
            if (valuesToBeChanged.ContainsKey(keyName))
            {
                valuesToBeChanged[keyName] = value;
            }
            else
            {
                valuesToBeChanged.Add(keyName, value);
            }
        }
    }

    public void ApplySettings()
    {
        foreach (KeyValuePair<string, object> changeToBeMade in valuesToBeChanged)
        {
            OptionsHandler.SetSetting(keys, changeToBeMade.Key, changeToBeMade.Value);
        }

        valuesToBeChanged.Clear();
    }

    public bool ChangesAreAwaiting()
    {
        return valuesToBeChanged.Count > 0;
    }

    public void CancelChanges()
    {
        valuesToBeChanged.Clear();

        if (EventHandler.OnKeybindsChanged != null)
        {
            EventHandler.OnKeybindsChanged.Invoke();
        }
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/controls.txt")) CheckFileDirectories();

        OptionsHandler.WriteToFile(keys, Application.persistentDataPath + "/settings/controls.txt");
    }
}