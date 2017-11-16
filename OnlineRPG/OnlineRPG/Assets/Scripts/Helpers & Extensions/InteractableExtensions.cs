using System.Collections.Generic;

public static class InteractableExtensions
{
    public static List<Option> Format(this List<Option> interactOptions, Interactable interactable)
    {
        interactOptions.ForEach((option) =>
        {
            option.Text = option.Text.Replace("<name>", $"<color=green>{interactable.interactableName}</color>");
        });

        return interactOptions;
    }

    public static List<Option> FormatItem(this List<Option> interactOptions, ItemType item)
    {
        interactOptions.ForEach((option) =>
        {
            option.Text = option.Text.Replace("<name>", $"<color=orange>{item.itemName}</color>");
        });

        return interactOptions;
    }
}