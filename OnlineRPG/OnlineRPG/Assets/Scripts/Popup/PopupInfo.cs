using System.Collections.Generic;

public class PopupInfo
{
    public string Description;
    public List<PopupButtonInfo> Buttons;

    public PopupInfo(string description, List<PopupButtonInfo> buttons)
    {
        this.Description = description;
        this.Buttons = buttons;
    }
}