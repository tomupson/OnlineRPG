using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> audioSettings;
    Dictionary<string, IOptionsInfo> defaultAudioSettings;

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
        OptionsHelper.SetSetting(audioSettings, setting, value);
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

        defaultAudioSettings["MASTER_VOLUME"] = new SliderInfo("Master Volume", 100, 0, 100);
    }

    void LoadAudioSettings()
    {
        audioSettings = new Dictionary<string, IOptionsInfo>();

        audioSettings["MASTER_VOLUME"] = new SliderInfo("Master Volume", null, 0, 100);

        CheckFileDirectories();

        OptionsHelper.LoadFromSettings(audioSettings, defaultAudioSettings, Application.persistentDataPath + "/settings/audio.txt");

        ApplySettings();
    }

    public void ApplySettings()
    {
        // Set volume here.

        WriteToFile();
    }

    public void ResetAudioSettings()
    {
        OptionsHelper.ResetSettings(audioSettings, defaultAudioSettings);

        ApplySettings();

        WriteToFile();
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/audio.txt")) CheckFileDirectories();

        OptionsHelper.WriteToFile(audioSettings, Application.persistentDataPath + "/settings/audio.txt");
    }
}