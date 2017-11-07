using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : Interactable, IInteractable
{
    [HideInInspector] public List<Option> interactOptions { get; set; }

    void Start()
    {
        InitializeOptions();
    }

    public void OpenContextMenu()
    {
        if (interactOptions != null)
        {
            InteractionManager.singleton.SetOptions(interactOptions, true);
            InteractionManager.singleton.Show();
        } else
        {
            Debug.LogError("Interact Options have not been set!");
        }
    }

    new public void InitializeOptions()
    {
        base.InitializeOptions();

        interactOptions = new List<Option>();
        interactOptions.AddRange(base.baseInteractOptions);
        interactOptions.Add(new Option() { Text = "Talk to <name>", OnOptionClick = delegate { base.MoveToInteractable(TalkToNPC); } });
        interactOptions.Format(this);
    }

    void TalkToNPC()
    {
        Debug.Log("Started Dialogue with NPC");
        // TO DO: DIALOGUE
    }
}