using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TreeInteractable : Interactable, IInteractable
{
    [SerializeField] private float treeRegrowTimeMin;
    [SerializeField] private float treeRegrowTimeMax;
    [SerializeField] private GameObject treeGfx;
    [SerializeField] private GameObject stumpGfx;

    [Header("On Chop")]
    public float chopXpGain;
    public string chopItemSlug;
    public int chopItemAmount;

    Inventory inventory;
    [HideInInspector] public List<Option> interactOptions { get; set; } // These are specific to the -TREE- interactable.
    MeshCollider treeCollider;

    void Start()
    {
        InitializeOptions();
        inventory = FindObjectOfType<Inventory>();
        treeCollider = GetComponent<MeshCollider>();
    }

    new public void InitializeOptions()
    {
        base.InitializeOptions();

        interactOptions = new List<Option>();
        interactOptions.AddRange(base.baseInteractOptions); // The base interactable options (that all interactables should have) are set here.
        interactOptions.Add(
            new Option() { Text = "Chop <name>", OnOptionClick = delegate { base.MoveToInteractable(ChopTree); } }
            );
        interactOptions.Format(this);
    }

    public void OpenContextMenu()
    {
        if (interactOptions != null) {
            InteractionManager.singleton.SetOptions(interactOptions, true);
            InteractionManager.singleton.Show();
        }
        else
        {
            Debug.LogError("Interact Options have not been set!");
        }
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