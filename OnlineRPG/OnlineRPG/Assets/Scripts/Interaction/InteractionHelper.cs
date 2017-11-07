using System.Collections.Generic;

public static class InteractionHelper
{
    public static void FormatOptions(Interactable interactable, ref List<Option> interactOptions)
    {
        interactOptions.ForEach((option) =>
        {
            option.Text = option.Text.Replace("<name>", $"<color=green>{interactable.interactableName}</color>");
        });
    }
}