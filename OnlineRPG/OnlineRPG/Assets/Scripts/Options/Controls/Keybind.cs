using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Keybind : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text keybindNameText;
    [SerializeField] private TMP_Text keyText;

    [HideInInspector] public bool isEditing = false;

    string keybindDictionaryKey;
    InputManager inputMan;
    List<KeyCode> keyCodes = ((KeyCode[])Enum.GetValues(typeof(KeyCode))).ToList();
    KeyCode currentValue;

    public IOptionsInfo info { get; set; }

    bool setup = false;
    bool checkingForChange = false;

    void Start()
    {
        List<KeyCode> bannedKeys = inputMan.GetBannedKeys();
        keyCodes.RemoveAll(x => bannedKeys.Contains(x));
        EventHandler.OnKeybindsChanged += CheckForChange;
    }

    void Update()
    {
        if (isEditing)
        {
            foreach (KeyCode k in keyCodes)
            {
                if (Input.GetKeyDown(k))
                {
                    isEditing = false;
                    inputMan.SetKey(keybindDictionaryKey, k);
                    currentValue = k;
                    keyText.text = k.ToString();
                }
            }
        }
    }

    public void Setup(string keybindName)
    {
        inputMan = InputManager.singleton;
        keybindDictionaryKey = keybindName;
        KeybindInfo keybindInfo = inputMan.GetKey(keybindName);
        info = keybindInfo;
        currentValue = keybindInfo.Key;
        keybindNameText.text = keybindInfo.Name;
        keyText.text = keybindInfo.Key.ToString();
        setup = true;
    }

    public void EnableKeybindEditor()
    {
        if (!setup || checkingForChange) return;

        isEditing = true;
    }

    void CheckForChange()
    {
        checkingForChange = true;
        KeybindInfo keybindInfo = inputMan.GetKey(keybindDictionaryKey);

        if (keybindInfo.Key != currentValue)
        {
            currentValue = keybindInfo.Key;
            keyText.text = currentValue.ToString();
        }
        checkingForChange = false;
    }
}