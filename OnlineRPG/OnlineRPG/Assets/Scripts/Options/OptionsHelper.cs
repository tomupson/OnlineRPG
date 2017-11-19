using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

public static class OptionsHelper
{
    public static List<string> GetFormattedResolutions()
    {
        List<string> result = new List<string>();

        Resolution[] resolutions = Screen.resolutions;
        resolutions.ToList().ForEach((resolution) =>
        {
            result.Add($"{resolution.width}x{resolution.height} ({resolution.refreshRate}Hz)");
        });

        return result;
    }

    public static int GetIndexOfCurrentResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        return resolutions.ToList().IndexOf(Screen.currentResolution);
    }

    public static Resolution GetResolutionFromIndex(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (index > resolutions.Length - 1) return resolutions[GetIndexOfCurrentResolution()];

        return resolutions[index];
    }

    public static bool CheckIfSettingIsOriginal(Dictionary<string, IOptionsInfo> settings, string setting, object value)
    {
        if (settings.ContainsKey(setting))
        {
            IOptionsInfo info = settings[setting];
            if (info is ToggleInfo)
            {
                if (((ToggleInfo)info).IsChecked == (bool)value) return true;
                return false;
            }
            else if (info is DropdownInfo)
            {
                if (((DropdownInfo)info).Index == (int)value) return true;
                return false;
            }
            else if (info is SliderInfo)
            {
                if(((SliderInfo)info).Value == (float)value) return true;
                return false;
            }
            else if (info is KeybindInfo)
            {
                if (((KeybindInfo)info).Key == (KeyCode)value) return true;
                return false;
            }
        }
        return false;
    }
}