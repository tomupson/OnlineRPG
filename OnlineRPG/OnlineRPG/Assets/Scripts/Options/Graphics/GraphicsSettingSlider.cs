using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingSlider : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text settingNameText;
    [SerializeField] private Slider settingSlider;

    private GraphicsManager graphicsMan;
    private string settingDictionaryKey;
    private float currentValue;

    public IOptionsInfo info { get; set; }

    void Start()
    {
        EventHandler.OnGraphicsSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        graphicsMan = GraphicsManager.singleton;
        settingDictionaryKey = settingName;
        SliderInfo sliderInfo = graphicsMan.GetSetting(settingName) as SliderInfo;
        info = sliderInfo;
        currentValue = (float)sliderInfo.Value;
        settingNameText.text = sliderInfo.Name;
        settingSlider.minValue = sliderInfo.MinValue;
        settingSlider.maxValue = sliderInfo.MaxValue;
        settingSlider.value = (float)sliderInfo.Value;
    }

    public void OnSliderValueChanged(float newValue)
    {
        graphicsMan.SetSetting(settingDictionaryKey, newValue);
        currentValue = newValue;
    }

    void CheckForChange()
    {
        SliderInfo dropdownInfo = graphicsMan.GetSetting(settingDictionaryKey) as SliderInfo;

        if ((float)dropdownInfo.Value != currentValue)
        {
            currentValue = (float)dropdownInfo.Value;
            settingSlider.value = currentValue;
        }
    }
}