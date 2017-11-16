using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingToggle : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text settingNameText;
    [SerializeField] private Toggle settingToggle;

    private GraphicsManager graphicsMan;
    private string settingDictionaryKey;
    private bool currentValue;

    void Start()
    {
        EventHandler.OnGraphicsSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        graphicsMan = GraphicsManager.singleton;
        settingDictionaryKey = settingName;
        ToggleInfo toggleInfo = graphicsMan.GetSetting(settingName) as ToggleInfo;
        currentValue = (bool)toggleInfo.IsChecked;
        settingNameText.text = toggleInfo.Name;
        settingToggle.isOn = currentValue;
    }

    public void OnCheckboxValueChanged(bool newState)
    {
        graphicsMan.SetSetting(settingDictionaryKey, newState);
        currentValue = newState;
    }

    void CheckForChange()
    {
        ToggleInfo toggleInfo = graphicsMan.GetSetting(settingDictionaryKey) as ToggleInfo;

        if ((bool)toggleInfo.IsChecked != currentValue)
        {
            currentValue = (bool)toggleInfo.IsChecked;
            settingToggle.isOn = currentValue;
        }
    }
}