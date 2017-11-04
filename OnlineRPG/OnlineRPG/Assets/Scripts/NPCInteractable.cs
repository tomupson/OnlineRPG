using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : Interactable
{
    public List<Option> interactOptions;

    public override void OpenContextMenu()
    {
        InteractionHelper.FormatOptions(this, ref interactOptions);
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