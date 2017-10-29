using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TreeInteractable : Interactable
{
    [SerializeField] private List<Option> interactOptions;
    [SerializeField] private float treeRegrowTimeMin;
    [SerializeField] private float treeRegrowTimeMax;
    [SerializeField] private GameObject treeGfx;
    [SerializeField] private GameObject stumpGfx;

    public override void OpenContextMenu()
    {
        interactOptions.ForEach((i) =>
        {
            i.text = i.text.Replace("<name>", interactableName);
        });

        InteractionManager.instance.SetOptions(interactOptions);
        base.OpenContextMenu();
    }

    public void MoveToTree()
    {
        MoveToInteractable(ChopTree);
    }

    void ChopTree()
    {
        Debug.Log("Chopping Tree");
        StartCoroutine(GrowTree());
        //treeGfx.SetActive(false);

        if (!string.IsNullOrEmpty(skillName))
        {
            BaseSkill skill = player.GetComponent<CharacterStats>().skills.Where(x => x.Name == skillName).FirstOrDefault();
            SkillManager.instance.GrantXPToSkill(skill, xpGain);
        }

        //stumpGfx.SetActive(true);
        inventory.AddItem(itemToAddSlug);
    }

    IEnumerator GrowTree()
    {
        yield return new WaitForSeconds(Random.Range(treeRegrowTimeMin, treeRegrowTimeMax));
        //treeGfx.SetActive(true);
        //stumpGfx.SetActive(false);
    }
}