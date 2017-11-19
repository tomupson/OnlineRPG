using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GeneralOptionsManager : MonoBehaviour, IOptionHandler
{
    #region Singleton
    public static GeneralOptionsManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> gameSettings;
    Dictionary<string, IOptionsInfo> defaultGameSettings;

    Dictionary<string, object> valuesToBeChanged = new Dictionary<string, object>();

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
        bool isOriginal = OptionsHelper.CheckIfSettingIsOriginal(gameSettings, setting, value);

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

        defaultGameSettings["SHOW_FPS"] = new ToggleInfo(string.Empty, string.Empty, false);
    }

    void LoadGameSettings()
    {
        gameSettings = new Dictionary<string, IOptionsInfo>();

        gameSettings["SHOW_FPS"] = new ToggleInfo("Show FPS", "Shows a small debug view of the current framerate.", null);

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
        foreach (KeyValuePair<string, object> changeToBeMade in valuesToBeChanged)
        {
            OptionsHandler.SetSetting(gameSettings, changeToBeMade.Key, changeToBeMade.Value);
        }

        valuesToBeChanged.Clear();

        FPSDisplay fps = FindObjectOfType<FPSDisplay>();
        if ((bool)((ToggleInfo)gameSettings["SHOW_FPS"]).IsChecked) fps.Show();
        else fps.Hide();

        WriteToFile();
    }

    public bool ChangesAreAwaiting()
    {
        return valuesToBeChanged.Count > 0;
    }

    public void CancelChanges()
    {
        valuesToBeChanged.Clear();

        if (EventHandler.OnGameSettingsChanged != null)
        {
            EventHandler.OnGameSettingsChanged.Invoke();
        }
    }

    public void ResetSettings()
    {
        PopupHandler.singleton.CreatePopup(new PopupInfo("Are you sure you want to reset your game settings?", new List<PopupButtonInfo>()
        {
            new PopupButtonInfo("Yes", ConfirmResetSettings),
            new PopupButtonInfo("No", delegate { Debug.Log("Cancelled"); })
        }));
    }

    public void ConfirmResetSettings()
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