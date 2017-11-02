using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : Interactable
{
    public List<Option> interactOptions;

    public override void OpenContextMenu()
    {
        interactOptions.ForEach((i) =>
        {
            i.text = i.text.Replace("<name>", interactableName);
        });

        InteractionManager.instance.SetOptions(interactOptions);

        base.OpenContextMenu();
    }

    public void MoveToInteractable()
    {
        base.MoveToInteractable(TalkToNPC);
    }

    void TalkToNPC()
    {
        Debug.Log("Started Dialogue with NPC");
        // TO DO: DIALOGUE
    }
}