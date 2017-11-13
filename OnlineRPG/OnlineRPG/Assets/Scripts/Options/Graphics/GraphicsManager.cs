using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GraphicsManager : MonoBehaviour
{
    #region Singleton
    public static GraphicsManager singleton;
    #endregion

    Dictionary<string, IOptionsInfo> graphicsSettings;
    Dictionary<string, IOptionsInfo> defaultGraphicsSettings;
    Dictionary<string, List<string>> settingDropdownOptions;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("More than one Graphics Manager on the client!");
            return;
        }

        singleton = this;

        CheckFileDirectories();
        SetDefaultGraphicsSettings();
        InitializeDropdownOptions();
        LoadGraphicsSettings();
    }

    void InitializeDropdownOptions()
    {
        settingDropdownOptions = new Dictionary<string, List<string>>();

        settingDropdownOptions.Add("RESOLUTION", OptionsHelper.GetFormattedResolutions());
        settingDropdownOptions.Add("ANTI_ALIASING", new List<string>()
        {
            "None",
            "Medium",
            "High"
        });
        settingDropdownOptions.Add("TEXTURE_QUALITY", new List<string>()
        {
            "Low",
            "Medium",
            "High"
        });
    }

    public IOptionsInfo GetSetting(string settingName)
    {
        if (graphicsSettings.ContainsKey(settingName))
        {
            return graphicsSettings[settingName];
        }

        return null;
    }

    public List<string> GetDropdownOptionsFor(string settingName)
    {
        if (settingDropdownOptions.ContainsKey(settingName))
        {
            return settingDropdownOptions[settingName];
        }

        return null;
    }

    public void SetSetting(string setting, object value)
    {
        if (graphicsSettings.ContainsKey(setting))
        {
            IOptionsInfo info = graphicsSettings[setting];
            if (info is ToggleInfo)
            {
                ((ToggleInfo)info).IsChecked = (bool)value;
            } else if (info is DropdownInfo)
            {
                ((DropdownInfo)info).Index = (int)value;
            } else if (info is SliderInfo)
            {
                ((SliderInfo)info).Value = (float)value;
            }
        }
    }

    public List<string> GetAllGraphicsSettings()
    {
        return graphicsSettings.Keys.ToList();
    }

    void CheckFileDirectories()
    {
        DirectoryHelper.singleton.CheckDirectories();

        if (!File.Exists(Application.persistentDataPath + "/settings/graphics.txt"))
        {
            File.CreateText(Application.persistentDataPath + "/settings/graphics.txt");
        }
    }

    void SetDefaultGraphicsSettings()
    {
        defaultGraphicsSettings = new Dictionary<string, IOptionsInfo>();

        defaultGraphicsSettings["FULLSCREEN"] = new ToggleInfo("Fullscreen", true);
        defaultGraphicsSettings["VSYNC"] = new ToggleInfo("VSync", true);
        defaultGraphicsSettings["RESOLUTION"] = new DropdownInfo("Resolution", OptionsHelper.GetIndexOfCurrentResolution());
        defaultGraphicsSettings["ANTI_ALIASING"] = new DropdownInfo("Anti Aliasing", 2);
        defaultGraphicsSettings["TEXTURE_QUALITY"] = new DropdownInfo("Texture Quality", 2);
    }

    void LoadGraphicsSettings()
    {
        graphicsSettings = new Dictionary<string, IOptionsInfo>();

        graphicsSettings["FULLSCREEN"] = new ToggleInfo("Fullscreen", null);
        graphicsSettings["VSYNC"] = new ToggleInfo("VSync", null);
        graphicsSettings["RESOLUTION"] = new DropdownInfo("Resolution", null);
        graphicsSettings["ANTI_ALIASING"] = new DropdownInfo("Anti Aliasing", null);
        graphicsSettings["TEXTURE_QUALITY"] = new DropdownInfo("Texture Quality", null);

        CheckFileDirectories();

        OptionsHelper.LoadFromSettings(graphicsSettings, defaultGraphicsSettings, Application.persistentDataPath + "/settings/graphics.txt");

        ApplySettings();
    }

    public void ApplySettings()
    {
        // 2 is 60fps on 60Hz, 144fps on 144Hz e.t.c. 0 is none. 1 is half refresh rate, which I'm not using.
        QualitySettings.vSyncCount = (bool)((ToggleInfo)graphicsSettings["VSYNC"]).IsChecked ? 2 : 0;
        Resolution newRes = OptionsHelper.GetResolutionFromIndex((int)((DropdownInfo)graphicsSettings["RESOLUTION"]).Index);
        Screen.SetResolution(newRes.width, newRes.height, (bool)((ToggleInfo)graphicsSettings["FULLSCREEN"]).IsChecked, newRes.refreshRate);
        QualitySettings.antiAliasing = (int)((DropdownInfo)graphicsSettings["ANTI_ALIASING"]).Index;
        // **Texture Quality thing here.**

        WriteToFile();
    }

    public void ResetGraphicsSettings()
    {
        OptionsHelper.ResetSettings(graphicsSettings, defaultGraphicsSettings);

        ApplySettings();

        WriteToFile();

        if (EventHandler.OnGraphicsSettingsChanged != null)
        {
            EventHandler.OnGraphicsSettingsChanged.Invoke();
        }
    }

    public void WriteToFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/settings/graphics.txt")) CheckFileDirectories();

        OptionsHelper.WriteToFile(graphicsSettings, Application.persistentDataPath + "/settings/graphics.txt");
    }
}