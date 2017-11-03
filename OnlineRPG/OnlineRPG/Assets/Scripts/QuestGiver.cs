using UnityEngine;

public class QuestGiver : NPCInteractable
{
    Quest quest;

    bool hasGivenQuest = false;

    void Start()
    {
        quest = GetComponent<Quest>();
        hasGivenQuest = false;
    }

    public void GiveQuest()
    {
        MoveToInteractable(GrantQuest);
    }

    void GrantQuest()
    {
        if (!hasGivenQuest)
        {
            if (quest != null)
            {
                Debug.Log("Given Player Quest: " + quest.Name);
                FindObjectOfType<Player>().GetComponent<CharacterStats>().AddQuest(quest);
                hasGivenQuest = true;
            }
        }
    }
}