using TMPro;
using UnityEngine;

public class GraphicsSettingDropdown : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text settingNameText;
    [SerializeField] private TMP_Dropdown settingDropdown;

    private GraphicsManager graphicsMan;
    private string settingDictionaryKey;
    private int currentValue;

    void Start()
    {
        EventHandler.OnGraphicsSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        graphicsMan = GraphicsManager.singleton;
        settingDictionaryKey = settingName;
        DropdownInfo dropdownInfo = graphicsMan.GetSetting(settingName) as DropdownInfo;
        currentValue = (int)dropdownInfo.Index;
        settingNameText.text = dropdownInfo.Name;
        settingDropdown.AddOptions(graphicsMan.GetDropdownOptionsFor(settingDictionaryKey));
        settingDropdown.value = (int)dropdownInfo.Index;
    }

    public void OnDropdownValueChanged(int newIndex)
    {
        graphicsMan.SetSetting(settingDictionaryKey, newIndex);
        currentValue = newIndex;
    }

    void CheckForChange()
    {
        DropdownInfo dropdownInfo = graphicsMan.GetSetting(settingDictionaryKey) as DropdownInfo;
        
        if ((int)dropdownInfo.Index != currentValue)
        {
            currentValue = (int)dropdownInfo.Index;
            settingDropdown.value = currentValue;
        }
    }
}