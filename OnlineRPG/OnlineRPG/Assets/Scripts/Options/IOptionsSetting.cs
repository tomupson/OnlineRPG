using System;

//[Obsolete("Graphics Settings components are now fetched depenending on the dictionary info type.")]
public interface IOptionsSetting
{
    IOptionsInfo info { get; set; }
    void Setup(string settingName);
}