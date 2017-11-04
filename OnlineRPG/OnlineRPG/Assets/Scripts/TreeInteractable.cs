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

    MeshCollider treeCollider;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        treeCollider = GetComponent<MeshCollider>();
    }

    public override void OpenContextMenu()
    {
        InteractionHelper.FormatOptions(this, ref interactOptions);

        InteractionManager.instance.SetOptions(interactOptions);
        base.OpenContextMenu();
    }

    public void MoveToInteractable()
    {
        base.MoveToInteractable(ChopTree);
    }

    void ChopTree()
    {
        treeGfx.SetActive(false);
        treeCollider.enabled = false;
        stumpGfx.SetActive(true);

        StartCoroutine(GrowTree());
        BaseSkill skill = player.stats.skills.Where(x => x.Name == "Woodcutting").FirstOrDefault();
        SkillManager.singleton.GrantXPToSkill(skill, chopXpGain);

        inventory.AddItem(chopItemSlug, chopItemAmount);
    }

    IEnumerator GrowTree()
    {
        yield return new WaitForSeconds(Random.Range(treeRegrowTimeMin, treeRegrowTimeMax));
        treeGfx.SetActive(true);
        treeCollider.enabled = true;
        stumpGfx.SetActive(false);
    }
}