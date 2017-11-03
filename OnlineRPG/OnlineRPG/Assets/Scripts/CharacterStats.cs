using UnityEngine;
using System.Collections.Generic;

public class CharacterStats : MonoBehaviour
{
    public List<BaseSkill> skills;
    public List<Quest> quests;

    public delegate void OnQuestAddedDelegate(Quest questAdded);
    public OnQuestAddedDelegate OnQuestAdded;

    void Start()
    {
        skills.ForEach((i) =>
        {
            SkillManager.singleton.CreateSkillFor(i);
        });
    }

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
        
        if (OnQuestAdded != null)
        {
            OnQuestAdded.Invoke(quest);
        }
    }
}