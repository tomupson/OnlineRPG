using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Interactable, IInteractable
{
    Quest quest;
    bool hasGivenQuest = false;
    [HideInInspector] public List<Option> interactOptions { get; set; }

    void Start()
    {
        InitializeOptions();
        quest = GetComponent<Quest>();
        hasGivenQuest = false;
    }

    new public void InitializeOptions()
    {
        base.InitializeOptions();

        interactOptions = new List<Option>();
        interactOptions.AddRange(base.baseInteractOptions);
        interactOptions.Add(new Option() { Text = "Get Quest from <name>", OnOptionClick = delegate { MoveToInteractable(GrantQuest); } });
        interactOptions.Format(this);
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

    void GrantQuest()
    {
        if (!hasGivenQuest)
        {
            if (quest != null)
            {
                Debug.Log("Given Player Quest: " + quest.Name);
                Player.LocalPlayer.GetComponent<CharacterStats>().AddQuest(quest);
                hasGivenQuest = true;
            }
        }
    }
}