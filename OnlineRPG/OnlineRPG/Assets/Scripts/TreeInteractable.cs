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

    Inventory inventory;

    [Header("On Chop")]
    public float chopXpGain;
    public string chopItemSlug;
    public int chopItemAmount;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

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
        base.MoveToInteractable(ChopTree);
    }

    void ChopTree()
    {
        StartCoroutine(GrowTree());
        //treeGfx.SetActive(false);
        BaseSkill skill = player.stats.skills.Where(x => x.Name == "Woodcutting").FirstOrDefault();
        SkillManager.singleton.GrantXPToSkill(skill, chopXpGain);

        //stumpGfx.SetActive(true);
        inventory.AddItem(chopItemSlug, chopItemAmount);
    }

    IEnumerator GrowTree()
    {
        yield return new WaitForSeconds(Random.Range(treeRegrowTimeMin, treeRegrowTimeMax));
        //treeGfx.SetActive(true);
        //stumpGfx.SetActive(false);
    }
}