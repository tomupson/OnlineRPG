using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager singleton;
    #endregion

    Dictionary<string, KeybindInfo> keys;
    List<KeyCode> bannedKeybindKeys;

    private Dictionary<string, KeyCode> defaultKeybinds;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one InputManager on the client!");
            return;
        }

        singleton = this;

        CreateFileDirectories();
        InitializeBannedKeys();
        SetDefaultKeys();
        LoadKeybinds();
    }

    void CreateFileDirectories()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/settings"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/settings");
        }

        if (!File.Exists(Application.persistentDataPath + "/settings/controls.txt"))
        {
            File.CreateText(Application.persistentDataPath + "/settings/controls.txt");
        }
    }

    // Default KeyCodes for each input type in the dictionary.
    void SetDefaultKeys()
    {
        // Set Default Keys in case keybinds file is changed to illegal keys or corrupted by the user.
        defaultKeybinds = new Dictionary<string, KeyCode>();

        defaultKeybinds["OPEN_INVENTORY"] = KeyCode.I;
        defaultKeybinds["OPEN_SKILLS"] = KeyCode.L;
        defaultKeybinds["OPEN_QUESTS"] = KeyCode.Q;
        defaultKeybinds["OPEN_CHAT"] = KeyCode.T;
        defaultKeybinds["OPEN_PAUSE_MENU"] = KeyCode.Escape;
        defaultKeybinds["TAKE_SCREENSHOT"] = KeyCode.F12;
    }

    // Loads the KeyCodes in from the file.
    void LoadKeybinds()
    {
        // By default everything is KeyCode.None.
        keys = new Dictionary<string, KeybindInfo>();
        keys["OPEN_INVENTORY"] = new KeybindInfo("Inventory", KeyCode.None);
        keys["OPEN_SKILLS"] = new KeybindInfo("Skills", KeyCode.None);
        keys["OPEN_QUESTS"] = new KeybindInfo("Quest Book", KeyCode.None);
        keys["OPEN_CHAT"] = new KeybindInfo("Chat", KeyCode.None);
        keys["OPEN_PAUSE_MENU"] = new KeybindInfo("Pause Menu", KeyCode.None);
        keys["TAKE_SCREENSHOT"] = new KeybindInfo("Screenshot", KeyCode.None);

        // Recreate file directories if they are deleted. As "CreateFileDirectories" is called before anyway, this will most likely never run.
        if (!Directory.Exists(Application.persistentDataPath + "/settings") ||
            !File.Exists(Application.persistentDataPath + "/settings/controls.txt")) CreateFileDirectories();

        string line;

        // Regex for matching "x=y", where x (SHOULD) be the input type and y (SHOULD) be the KeyCode.
        Regex rgx = new Regex(@"^\w+=[\w\d]+$");

        // Read the file
        StreamReader file = new StreamReader(Application.persistentDataPath + "/settings/controls.txt");

        // Keeps track of whether keys were missing from the file or the keybinds were there but the keycodes were invalid or banned.
        bool fileHasErrors = false;
        while ((line = file.ReadLine()) != null)
        {
            // If the line matches the regex.
            if (rgx.IsMatch(line))
            {
                string[] parts = line.Split('=');
                if (keys.ContainsKey(parts[0]))
                {
                    KeyCode keyCode = KeyCode.None;
                    Enum.TryParse(parts[1], out keyCode);
                    // If the keycode succeeded in parsing and it's not a banned keycode.
                    if (keyCode != default(KeyCode) && !bannedKeybindKeys.Contains(keyCode))
                    {
                        // Assign it.
                        keys[parts[0]].Key = keyCode;
                    } else if (defaultKeybinds.ContainsKey(parts[0]))
                    {
                        // Otherwise, ensure the default keybinds has that input type and assign the default.
                        keys[parts[0]].Key = defaultKeybinds[parts[0]];
                        // This means there was an invalid or banned KeyCode, so errors were found.
                        fileHasErrors = true;
                    }
                }
            }
        }

        file.Close();

        // Go through each keybind we've registered before opening the file.
        foreach (KeyValuePair<string, KeybindInfo> keybind in keys)
        {
            // If the key STILL hasn't been assigned
            if (keybind.Value.Key == KeyCode.None)
            {
                // Set it to the default keybind.
                keybind.Value.Key = defaultKeybinds[keybind.Key];

                // This also means we need a rewrite.
                fileHasErrors = true;
            }
        }

        if (fileHasErrors)
        {
            // If errors were found, write back to the file.
            WriteToFile();
        }
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
        foreach (KeyValuePair<string, KeybindInfo> keybind in keys)
        {
            if (defaultKeybinds.ContainsKey(keybind.Key))
            {
                keybind.Value.Key = defaultKeybinds[keybind.Key];
            }
        }

        WriteToFile();

        if (EventHandler.OnKeybindsReset != null)
        {
            EventHandler.OnKeybindsReset.Invoke();
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
            return keys[keyName];
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

    public void SetKey(string keyName, KeyCode newKey)
    {
        if (keys.ContainsKey(keyName))
        {
            keys[keyName].Key = newKey;
            Debug.Log($"Successfully updated key binding for {keyName} to {newKey.ToString()}");
        }
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/controls.txt")) CreateFileDirectories();

        using (StreamWriter file = new StreamWriter(Application.persistentDataPath + "/settings/controls.txt"))
        {
            foreach (KeyValuePair<string, KeybindInfo> keyBind in keys)
            {
                file.WriteLine($"{keyBind.Key}={keyBind.Value.Key.ToString()}");
            }
        }
    }
}