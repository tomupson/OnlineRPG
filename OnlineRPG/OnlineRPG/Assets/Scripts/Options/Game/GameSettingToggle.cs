using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingToggle : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text settingNameText;
    [SerializeField] private Toggle settingToggle;

    private GeneralOptionsManager generalMan;
    private string settingDictionaryKey;
    private bool currentValue;

    public IOptionsInfo info { get; set; }

    bool setup = false;
    bool checkingForChange = false;

    void Start()
    {
        EventHandler.OnGameSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        generalMan = GeneralOptionsManager.singleton;
        settingDictionaryKey = settingName;
        ToggleInfo toggleInfo = generalMan.GetSetting(settingName) as ToggleInfo;
        info = toggleInfo;
        currentValue = (bool)toggleInfo.IsChecked;
        settingNameText.text = toggleInfo.Name;
        settingToggle.isOn = currentValue;
        setup = true;
    }

    public void OnCheckboxValueChanged(bool newState)
    {
        if (!setup || checkingForChange) return;

        generalMan.SetSetting(settingDictionaryKey, newState);
        currentValue = newState;
    }

    void CheckForChange()
    {
        checkingForChange = true;
        ToggleInfo toggleInfo = generalMan.GetSetting(settingDictionaryKey) as ToggleInfo;

        if ((bool)toggleInfo.IsChecked != currentValue)
        {
            currentValue = (bool)toggleInfo.IsChecked;
            settingToggle.isOn = currentValue;
        }
        checkingForChange = false;
    }
}