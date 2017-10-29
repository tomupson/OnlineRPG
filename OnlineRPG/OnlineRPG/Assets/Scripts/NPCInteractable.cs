using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCInteractable : Interactable
{
    [SerializeField] private List<Option> interactOptions;

    public override void OpenContextMenu()
    {
        interactOptions.ForEach((i) =>
        {
            i.text = i.text.Replace("<name>", interactableName);
        });

        InteractionManager.instance.SetOptions(interactOptions);
        base.OpenContextMenu();
    }

    public void MoveToNPC()
    {
        MoveToInteractable(TalkToNPC);
    }

    void TalkToNPC()
    {
        Debug.Log("Started Dialogue with NPC");
        BaseSkill skill = player.GetComponent<CharacterStats>().skills.Where(x => x.Name == skillName).FirstOrDefault();
        SkillManager.instance.GrantXPToSkill(skill, xpGain);
        inventory.AddItem(itemToAddSlug);
    }
}