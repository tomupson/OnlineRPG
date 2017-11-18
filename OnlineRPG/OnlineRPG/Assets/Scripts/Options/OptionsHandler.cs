using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class OptionsHandler
{
    public static void LoadFromSettings(Dictionary<string, IOptionsInfo> settings, Dictionary<string, IOptionsInfo> defaults, string path)
    {
        string line;

        Regex rgx = new Regex(@"^\w+=[\w\d]+$");

        StreamReader file = new StreamReader(path);

        // Keeps track of whether keys were missing from the file or the keybinds were there but the keycodes were invalid or banned.
        bool fileHasErrors = false;
        while ((line = file.ReadLine()) != null)
        {
            // If the line matches the regex.
            if (rgx.IsMatch(line))
            {
                string[] parts = line.Split('=');
                if (settings.ContainsKey(parts[0]))
                {
                    IOptionsInfo info = settings[parts[0]];
                    if (info is ToggleInfo)
                    {
                        bool val;
                        bool.TryParse(parts[1], out val);

                        if (val != default(bool))
                        {
                            ((ToggleInfo)info).IsChecked = val;
                        }
                        else if (defaults.ContainsKey(parts[0]))
                        {
                            ((ToggleInfo)info).IsChecked = ((ToggleInfo)defaults[parts[0]]).IsChecked;
                            fileHasErrors = true;
                        }
                    }
                    else if (info is DropdownInfo)
                    {
                        int val;
                        int.TryParse(parts[1], out val);

                        if (val != default(int))
                        {
                            ((DropdownInfo)info).Index = val;
                        }
                        else if (defaults.ContainsKey(parts[0]))
                        {
                            ((DropdownInfo)info).Index = ((DropdownInfo)defaults[parts[0]]).Index;
                            fileHasErrors = true;
                        }
                    }
                    else if (info is SliderInfo)
                    {
                        SliderInfo sliderInfo = ((SliderInfo)info);

                        float val;
                        float.TryParse(parts[1], out val);

                        if (val != default(float) && !(sliderInfo.Value > sliderInfo.MaxValue) &&
                            !(sliderInfo.Value < sliderInfo.MinValue) &&
                            !(sliderInfo.Value < 0))
                        {
                            ((SliderInfo)info).Value = val;
                        }
                        else if (defaults.ContainsKey(parts[0]))
                        {
                            ((SliderInfo)info).Value = ((SliderInfo)defaults[parts[0]]).Value;
                            ((SliderInfo)info).MinValue = ((SliderInfo)defaults[parts[0]]).MinValue;
                            ((SliderInfo)info).MaxValue = ((SliderInfo)defaults[parts[0]]).MaxValue;
                        }
                    }
                    else if (info is KeybindInfo)
                    {
                        KeyCode val;
                        Enum.TryParse(parts[1], out val);
                        // If the keycode succeeded in parsing and it's not a banned keycode.
                        if (val != default(KeyCode) && !InputManager.singleton.GetBannedKeys().Contains(val))
                        {
                            // Assign it.
                            ((KeybindInfo)info).Key = val;
                        }
                        else if (defaults.ContainsKey(parts[0]))
                        {
                            // Otherwise, ensure the default keybinds has that input type and assign the default.
                            ((KeybindInfo)info).Key = ((KeybindInfo)defaults[parts[0]]).Key;
                            // This means there was an invalid or banned KeyCode, so errors were found.
                            fileHasErrors = true;
                        }
                    }
                }
            }
        }

        file.Close();

        // Go through each keybind we've registered before opening the file.
        foreach (KeyValuePair<string, IOptionsInfo> setting in settings)
        {
            IOptionsInfo info = setting.Value;

            if (info is ToggleInfo)
            {
                ToggleInfo graphicsInfo = ((ToggleInfo)info);
                if (graphicsInfo.IsChecked == null)
                {
                    graphicsInfo.IsChecked = ((ToggleInfo)defaults[setting.Key]).IsChecked;
                    fileHasErrors = true;
                }
            }
            else if (info is DropdownInfo)
            {
                DropdownInfo graphicsInfo = ((DropdownInfo)info);
                if (graphicsInfo.Index == null)
                {
                    graphicsInfo.Index = ((DropdownInfo)defaults[setting.Key]).Index;
                    fileHasErrors = true;
                }
            } else if (info is SliderInfo)
            {
                SliderInfo sliderInfo = ((SliderInfo)info);
                if (sliderInfo.Value == null)
                {
                    sliderInfo.Value = ((SliderInfo)defaults[setting.Key]).Value;
                    fileHasErrors = true;
                }
            } else if (info is KeybindInfo)
            {
                KeybindInfo keybindInfo = ((KeybindInfo)info);
                if (keybindInfo.Key == KeyCode.None)
                {
                    keybindInfo.Key = ((KeybindInfo)defaults[setting.Key]).Key;
                    fileHasErrors = true;
                }
            }
        }

        if (fileHasErrors)
        {
            // If errors were found, write back to the file.
            WriteToFile(settings, path);
        }
    }

    public static void WriteToFile(Dictionary<string, IOptionsInfo> settings, string path)
    {
        using (StreamWriter file = new StreamWriter(path))
        {
            foreach (KeyValuePair<string, IOptionsInfo> setting in settings)
            {
                IOptionsInfo info = settings[setting.Key];
                if (info is DropdownInfo)
                {
                    DropdownInfo dropdownInfo = info as DropdownInfo;
                    file.WriteLine($"{setting.Key}={dropdownInfo.Index.ToString()}");
                }
                else if (info is ToggleInfo)
                {
                    ToggleInfo toggleInfo = info as ToggleInfo;
                    file.WriteLine($"{setting.Key}={toggleInfo.IsChecked.ToString()}");
                }
                else if (info is SliderInfo)
                {
                    SliderInfo sliderInfo = info as SliderInfo;
                    file.WriteLine($"{setting.Key}={sliderInfo.Value.ToString()}");
                } else if (info is KeybindInfo)
                {
                    KeybindInfo keybindInfo = info as KeybindInfo;
                    file.WriteLine($"{setting.Key}={keybindInfo.Key.ToString()}");
                }
            }
        }
    }

    public static void SetSetting(Dictionary<string, IOptionsInfo> settings, string setting, object value)
    {
        if (settings.ContainsKey(setting))
        {
            IOptionsInfo info = settings[setting];
            if (info is ToggleInfo)
            {
                ((ToggleInfo)info).IsChecked = (bool)value;
            }
            else if (info is DropdownInfo)
            {
                ((DropdownInfo)info).Index = (int)value;
            }
            else if (info is SliderInfo)
            {
                ((SliderInfo)info).Value = (float)value;
            } else if (info is KeybindInfo)
            {
                ((KeybindInfo)info).Key = (KeyCode)value;
                Debug.Log($"Successfully updated key binding for {setting} to {value.ToString()}");
            }
        }
    }

    public static void ResetSettings(Dictionary<string, IOptionsInfo> settings, Dictionary<string, IOptionsInfo> defaults)
    {
        foreach (KeyValuePair<string, IOptionsInfo> setting in settings)
        {
            if (defaults.ContainsKey(setting.Key))
            {
                IOptionsInfo info = setting.Value;

                if (info is ToggleInfo)
                {
                    ((ToggleInfo)info).IsChecked = ((ToggleInfo)defaults[setting.Key]).IsChecked;
                }
                else if (info is DropdownInfo)
                {
                    ((DropdownInfo)info).Index = ((DropdownInfo)defaults[setting.Key]).Index;
                }
                else if (info is SliderInfo)
                {
                    ((SliderInfo)info).Value = ((SliderInfo)defaults[setting.Key]).Value;
                } else if (info is KeybindInfo)
                {
                    ((KeybindInfo)info).Key = ((KeybindInfo)defaults[setting.Key]).Key;
                }
            }
        }
    }
}