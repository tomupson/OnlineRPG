using System.Collections.Generic;

public static class InteractionHelper
{
    public static void FormatOptions(Interactable interactable, ref List<Option> interactionOptions)
    {
        interactionOptions.ForEach((option) =>
        {
            option.text = option.text.Replace("<name>", $"<color=green>{interactable.interactableName}</color>");
        });
    }
}