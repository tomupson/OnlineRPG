using UnityEngine;
using System.Collections.Generic;

public class CharacterStats : MonoBehaviour
{
    public List<BaseSkill> skills;

    void Start()
    {
        foreach (BaseSkill skill in skills)
        {
            SkillManager.instance.CreateSkillFor(skill);
        }
    }
}