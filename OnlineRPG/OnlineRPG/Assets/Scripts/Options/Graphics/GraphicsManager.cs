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
            "Off",
            "2x MSAA",
            "4x MSAA",
            "8x MSAA"
        });
        settingDropdownOptions.Add("TEXTURE_QUALITY", new List<string>()
        {
            "Low",
            "Medium",
            "High"
        });
        settingDropdownOptions.Add("SHADOWS", new List<string>()
        {
            "Off",
            "Low",
            "Medium",
            "High",
            "Ultra"
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
        OptionsHandler.SetSetting(graphicsSettings, setting, value);
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

        defaultGraphicsSettings["FULLSCREEN"] = new ToggleInfo("", true);
        defaultGraphicsSettings["VSYNC"] = new ToggleInfo("", true);
        defaultGraphicsSettings["RESOLUTION"] = new DropdownInfo("", OptionsHelper.GetIndexOfCurrentResolution());
        defaultGraphicsSettings["ANTI_ALIASING"] = new DropdownInfo("", 2);
        defaultGraphicsSettings["TEXTURE_QUALITY"] = new DropdownInfo("", 2);
        defaultGraphicsSettings["SHADOWS"] = new DropdownInfo("", 4);
    }

    void LoadGraphicsSettings()
    {
        graphicsSettings = new Dictionary<string, IOptionsInfo>();

        graphicsSettings["FULLSCREEN"] = new ToggleInfo("Fullscreen", null);
        graphicsSettings["VSYNC"] = new ToggleInfo("VSync", null);
        graphicsSettings["RESOLUTION"] = new DropdownInfo("Resolution", null);
        graphicsSettings["ANTI_ALIASING"] = new DropdownInfo("Anti Aliasing", null);
        graphicsSettings["TEXTURE_QUALITY"] = new DropdownInfo("Texture Quality", null);
        graphicsSettings["SHADOWS"] = new DropdownInfo("Shadows", null);

        CheckFileDirectories();

        OptionsHandler.LoadFromSettings(graphicsSettings, defaultGraphicsSettings, Application.persistentDataPath + "/settings/graphics.txt");

        ApplySettings();
    }

    public void ApplySettings()
    {
        QualitySettings.vSyncCount = (bool)((ToggleInfo)graphicsSettings["VSYNC"]).IsChecked ? 1 : 0;
        Resolution newRes = OptionsHelper.GetResolutionFromIndex((int)((DropdownInfo)graphicsSettings["RESOLUTION"]).Index);
        Screen.SetResolution(newRes.width, newRes.height, (bool)((ToggleInfo)graphicsSettings["FULLSCREEN"]).IsChecked, newRes.refreshRate);
        QualitySettings.antiAliasing = (int)((DropdownInfo)graphicsSettings["ANTI_ALIASING"]).Index;

        QualitySettings.masterTextureLimit = (settingDropdownOptions["TEXTURE_QUALITY"].Count - 1) - (int)((DropdownInfo)graphicsSettings["TEXTURE_QUALITY"]).Index;

        int shadowDropdownIndex = (int)((DropdownInfo)graphicsSettings["SHADOWS"]).Index;

        if (shadowDropdownIndex == 0) QualitySettings.shadows = ShadowQuality.Disable;
        else if (shadowDropdownIndex >= settingDropdownOptions["SHADOWS"].Count - 2) QualitySettings.shadows = ShadowQuality.All;
        else QualitySettings.shadows = ShadowQuality.HardOnly;

        QualitySettings.shadowResolution = (ShadowResolution)shadowDropdownIndex + 1;
        QualitySettings.shadowDistance = shadowDropdownIndex != 0 ? 40 * shadowDropdownIndex : 0;

        WriteToFile();
    }

    public void ResetGraphicsSettings()
    {
        OptionsHandler.ResetSettings(graphicsSettings, defaultGraphicsSettings);

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

        OptionsHandler.WriteToFile(graphicsSettings, Application.persistentDataPath + "/settings/graphics.txt");
    }
}