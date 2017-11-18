using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingSlider : MonoBehaviour, IOptionsSetting
{
    [SerializeField] private TMP_Text settingNameText;
    [SerializeField] private Slider settingSlider;

    private AudioManager audioMan;
    private string settingDictionaryKey { get; set; }
    private float currentValue;

    public IOptionsInfo info { get; set; }

    void Start()
    {
        EventHandler.OnAudioSettingsChanged += CheckForChange;
    }

    public void Setup(string settingName)
    {
        audioMan = AudioManager.singleton;
        settingDictionaryKey = settingName;
        SliderInfo sliderInfo = audioMan.GetSetting(settingName) as SliderInfo;
        info = sliderInfo;
        currentValue = (float)sliderInfo.Value;
        settingNameText.text = sliderInfo.Name;
        settingSlider.minValue = sliderInfo.MinValue;
        settingSlider.maxValue = sliderInfo.MaxValue;
        settingSlider.value = (int)sliderInfo.Value;
    }

    public void OnSliderValueChanged(float newValue)
    {
        audioMan.SetSetting(settingDictionaryKey, newValue);
        currentValue = newValue;
    }

    void CheckForChange()
    {
        SliderInfo sliderInfo = audioMan.GetSetting(settingDictionaryKey) as SliderInfo;

        if ((float)sliderInfo.Value != currentValue)
        {
            currentValue = (float)sliderInfo.Value;
            settingSlider.value = currentValue;
        }
    }
}