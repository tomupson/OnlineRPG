using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GeneralOptionsManager : MonoBehaviour
{
    #region Singleton
    public static GeneralOptionsManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> gameSettings;
    Dictionary<string, IOptionsInfo> defaultGameSettings;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one General Options Manager on the client!");
            return;
        }

        singleton = this;

        CheckFileDirectories();
        SetDefaultGameSettings();
        LoadGameSettings();
    }

    public IOptionsInfo GetSetting(string settingName)
    {
        if (gameSettings.ContainsKey(settingName))
        {
            return gameSettings[settingName];
        }

        return null;
    }

    public void SetSetting(string setting, object value)
    {
        OptionsHandler.SetSetting(gameSettings, setting, value);
    }

    void CheckFileDirectories()
    {
        DirectoryHelper.singleton.CheckDirectories();

        if (!File.Exists(Application.persistentDataPath + "/settings/game.txt"))
        {
            File.CreateText(Application.persistentDataPath + "/settings/game.txt");
        }
    }

    void SetDefaultGameSettings()
    {
        defaultGameSettings = new Dictionary<string, IOptionsInfo>();

        defaultGameSettings["SHOW_FPS"] = new ToggleInfo("", false);
    }

    void LoadGameSettings()
    {
        gameSettings = new Dictionary<string, IOptionsInfo>();

        gameSettings["SHOW_FPS"] = new ToggleInfo("Show FPS", null);

        CheckFileDirectories();

        OptionsHandler.LoadFromSettings(gameSettings, defaultGameSettings, Application.persistentDataPath + "/settings/game.txt");

        ApplySettings();
    }

    public List<string> GetAllGameSettings()
    {
        return gameSettings.Keys.ToList();
    }

    public void ApplySettings()
    {
        FPSDisplay fps = FindObjectOfType<FPSDisplay>();
        if ((bool)((ToggleInfo)gameSettings["SHOW_FPS"]).IsChecked) fps.Show();
        else fps.Hide();

        WriteToFile();
    }

    public void ResetSettings()
    {
        OptionsHandler.ResetSettings(gameSettings, defaultGameSettings);

        ApplySettings();

        WriteToFile();

        if (EventHandler.OnGameSettingsChanged != null)
        {
            EventHandler.OnGameSettingsChanged.Invoke();
        }
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/game.txt")) CheckFileDirectories();

        OptionsHandler.WriteToFile(gameSettings, Application.persistentDataPath + "/settings/game.txt");
    }
}