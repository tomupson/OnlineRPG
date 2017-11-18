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

        defaultGraphicsSettings["FULLSCREEN"] = new ToggleInfo(string.Empty, string.Empty, true);
        defaultGraphicsSettings["VSYNC"] = new ToggleInfo(string.Empty, string.Empty, true);
        defaultGraphicsSettings["RESOLUTION"] = new DropdownInfo(string.Empty, string.Empty, OptionsHelper.GetIndexOfCurrentResolution());
        defaultGraphicsSettings["ANTI_ALIASING"] = new DropdownInfo(string.Empty, string.Empty, 2);
        defaultGraphicsSettings["TEXTURE_QUALITY"] = new DropdownInfo(string.Empty, string.Empty, 2);
        defaultGraphicsSettings["SHADOWS"] = new DropdownInfo(string.Empty, string.Empty, 4);
        defaultGraphicsSettings["MOTION_BLUR"] = new ToggleInfo(string.Empty, string.Empty, true);
        defaultGraphicsSettings["MOTION_BLUR_AMOUNT"] = new SliderInfo(string.Empty, string.Empty, 50, 0, 100);
        defaultGraphicsSettings["AMBIENT_OCCLUSION"] = new ToggleInfo(string.Empty, string.Empty, false);
        defaultGraphicsSettings["BLOOM"] = new ToggleInfo(string.Empty, string.Empty, false);
        //defaultGraphicsSettings["BLOOM_AMOUNT"] = new SliderInfo(string.Empty, string.Empty, 50, 0, 100);
        //defaultGraphicsSettings["SUN_SHAFTS"] = new ToggleInfo(string.Empty, string.Empty, true);
    }

    void LoadGraphicsSettings()
    {
        graphicsSettings = new Dictionary<string, IOptionsInfo>();

        graphicsSettings["FULLSCREEN"] = new ToggleInfo("Fullscreen", "Makes the game fullscreen.", null);
        graphicsSettings["VSYNC"] = new ToggleInfo("VSync", "Limits the FPS to your monitor's refresh rate to prevent screen tear.", null);
        graphicsSettings["RESOLUTION"] = new DropdownInfo("Resolution", "Adjusts the size of the game on your monitor.", null);
        graphicsSettings["ANTI_ALIASING"] = new DropdownInfo("Anti Aliasing", "Smooths jagged edges.", null);
        graphicsSettings["TEXTURE_QUALITY"] = new DropdownInfo("Texture Quality", "Changes the quality of the textures used in the game.", null);
        graphicsSettings["SHADOWS"] = new DropdownInfo("Shadows", "Changes the quality of the shadows.", null);
        graphicsSettings["MOTION_BLUR"] = new ToggleInfo("Motion Blur", "Blurs movement in the game.", null);
        graphicsSettings["MOTION_BLUR_AMOUNT"] = new SliderInfo("Motion Blur Amount", "Adjusts the amount of blur that occurs on motion.", null, 0, 100);
        graphicsSettings["AMBIENT_OCCLUSION"] = new ToggleInfo("Ambient Occlusion", "Adjusts the lighting on surfaces to replicate how light would penetrate in real life.", null);
        graphicsSettings["BLOOM"] = new ToggleInfo("Bloom", "Increases the brightness of light in spots where light would spread.", null);
        //graphicsSettings["BLOOM_AMOUNT"] = new SliderInfo("Bloom Amount", null, 0, 100);
        //graphicsSettings["SUN_SHAFTS"] = new ToggleInfo("Sun Shafts", null);

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