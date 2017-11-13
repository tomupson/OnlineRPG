using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Keybind : MonoBehaviour
{
    [SerializeField] private TMP_Text keybindNameText;
    [SerializeField] private TMP_Text keyText;

    [HideInInspector] public bool isEditing = false;

    string keybindDictionaryKey;
    InputManager inputMan;
    List<KeyCode> keyCodes = ((KeyCode[])Enum.GetValues(typeof(KeyCode))).ToList();
    KeyCode currentValue;

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
        currentValue = keybindInfo.Key;
        keybindNameText.text = keybindInfo.Name;
        keyText.text = keybindInfo.Key.ToString();
    }

    public void EnableKeybindEditor()
    {
        isEditing = true;
    }

    void CheckForChange()
    {
        KeybindInfo keybindInfo = inputMan.GetKey(keybindDictionaryKey);
        if (keybindInfo.Key != currentValue)
        {
            currentValue = keybindInfo.Key;
            keyText.text = currentValue.ToString();
        }
    }
}