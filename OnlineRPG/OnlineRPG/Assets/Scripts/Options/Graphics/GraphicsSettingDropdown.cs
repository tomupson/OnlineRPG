using TMPro;
using UnityEngine;

public class GraphicsSettingDropdown : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text settingNameText;
    [SerializeField] private TMP_Dropdown settingDropdown;

    private GraphicsManager graphicsMan;
    private string settingDictionaryKey;
    private int currentValue;

    public IOptionsInfo info { get; set; }

    bool setup = false;
    bool checkingForChange = false;

    void Start()
    {
        EventHandler.OnGraphicsSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        graphicsMan = GraphicsManager.singleton;
        settingDictionaryKey = settingName;
        DropdownInfo dropdownInfo = graphicsMan.GetSetting(settingName) as DropdownInfo;
        info = dropdownInfo;
        currentValue = (int)dropdownInfo.Index;
        settingNameText.text = dropdownInfo.Name;
        settingDropdown.AddOptions(graphicsMan.GetDropdownOptionsFor(settingDictionaryKey));
        settingDropdown.value = (int)dropdownInfo.Index;
        setup = true;
    }

    public void OnDropdownValueChanged(int newIndex)
    {
        if (!setup || checkingForChange) return;
        graphicsMan.SetSetting(settingDictionaryKey, newIndex);
        currentValue = newIndex;
    }

    void CheckForChange()
    {
        checkingForChange = true;
        DropdownInfo dropdownInfo = graphicsMan.GetSetting(settingDictionaryKey) as DropdownInfo;
        
        if ((int)dropdownInfo.Index != currentValue)
        {
            currentValue = (int)dropdownInfo.Index;
            settingDropdown.value = currentValue;
        }
        checkingForChange = false;
    }
}