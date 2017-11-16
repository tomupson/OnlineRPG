using System.Linq;
using UnityEngine;
using System.Collections.Generic;

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
}