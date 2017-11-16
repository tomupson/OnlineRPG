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

    void Start()
    {
        EventHandler.OnGameSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        generalMan = GeneralOptionsManager.singleton;
        settingDictionaryKey = settingName;
        ToggleInfo toggleInfo = generalMan.GetSetting(settingName) as ToggleInfo;
        currentValue = (bool)toggleInfo.IsChecked;
        settingNameText.text = toggleInfo.Name;
        settingToggle.isOn = currentValue;
    }

    public void OnCheckboxValueChanged(bool newState)
    {
        generalMan.SetSetting(settingDictionaryKey, newState);
        currentValue = newState;
    }

    void CheckForChange()
    {
        ToggleInfo toggleInfo = generalMan.GetSetting(settingDictionaryKey) as ToggleInfo;

        if ((bool)toggleInfo.IsChecked != currentValue)
        {
            currentValue = (bool)toggleInfo.IsChecked;
            settingToggle.isOn = currentValue;
        }
    }
}