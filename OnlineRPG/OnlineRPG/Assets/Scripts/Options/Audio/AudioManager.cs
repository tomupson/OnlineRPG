using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour, IOptionHandler
{
    #region Singleton
    public static AudioManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> audioSettings;
    Dictionary<string, IOptionsInfo> defaultAudioSettings;

    Dictionary<string, object> valuesToBeChanged = new Dictionary<string, object>();

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one Audio Manager on the client!");
            return;
        }

        singleton = this;

        CheckFileDirectories();
        SetDefaultAudioSettings();
        LoadAudioSettings();
    }

    public IOptionsInfo GetSetting(string settingName)
    {
        if (audioSettings.ContainsKey(settingName))
        {
            return audioSettings[settingName];
        }

        return null;
    }

    public void SetSetting(string setting, object value)
    {
        bool isOriginal = OptionsHelper.CheckIfSettingIsOriginal(audioSettings, setting, value);

        if (isOriginal) return;
        else
        {
            if (valuesToBeChanged.ContainsKey(setting))
            {
                valuesToBeChanged[setting] = value;
            }
            else
            {
                valuesToBeChanged.Add(setting, value);
            }
        }
    }

    public List<string> GetAllAudioSettings()
    {
        return audioSettings.Keys.ToList();
    }

    void CheckFileDirectories()
    {
        DirectoryHelper.singleton.CheckDirectories();

        if (!File.Exists(Application.persistentDataPath + "/settings/audio.txt"))
        {
            File.CreateText(Application.persistentDataPath + "/settings/audio.txt");
        }
    }

    void SetDefaultAudioSettings()
    {
        defaultAudioSettings = new Dictionary<string, IOptionsInfo>();

        defaultAudioSettings["MASTER_VOLUME"] = new SliderInfo(string.Empty, string.Empty, 100, 0, 100);
    }

    void LoadAudioSettings()
    {
        audioSettings = new Dictionary<string, IOptionsInfo>();

        audioSettings["MASTER_VOLUME"] = new SliderInfo("Master Volume", "Overall volume of the game.", null, 0, 100);

        CheckFileDirectories();

        OptionsHandler.LoadFromSettings(audioSettings, defaultAudioSettings, Application.persistentDataPath + "/settings/audio.txt");

        ApplySettings();
    }

    public void ApplySettings()
    {
        foreach (KeyValuePair<string, object> changeToBeMade in valuesToBeChanged)
        {
            OptionsHandler.SetSetting(audioSettings, changeToBeMade.Key, changeToBeMade.Value);
        }

        valuesToBeChanged.Clear();

        // Set volume here.

        WriteToFile();
    }

    public bool ChangesAreAwaiting()
    {
        return valuesToBeChanged.Count > 0;
    }

    public void CancelChanges()
    {
        valuesToBeChanged.Clear();

        if (EventHandler.OnAudioSettingsChanged != null)
        {
            EventHandler.OnAudioSettingsChanged.Invoke();
        }
    }

    public void ResetAudioSettings()
    {
        PopupHandler.singleton.CreatePopup(new PopupInfo("Are you sure you want to reset your audio settings?", new List<PopupButtonInfo>()
        {
            new PopupButtonInfo("Yes", ConfirmResetAudioSettings),
            new PopupButtonInfo("No", delegate { Debug.Log("Cancelled"); })
        }));
    }

    public void ConfirmResetAudioSettings()
    {
        OptionsHandler.ResetSettings(audioSettings, defaultAudioSettings);

        ApplySettings();

        WriteToFile();
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/audio.txt")) CheckFileDirectories();

        OptionsHandler.WriteToFile(audioSettings, Application.persistentDataPath + "/settings/audio.txt");
    }
}