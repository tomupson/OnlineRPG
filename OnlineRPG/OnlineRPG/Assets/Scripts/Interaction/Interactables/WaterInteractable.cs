using UnityEngine;
using System.Collections.Generic;

public class WaterInteractable : Interactable, IInteractable
{
    [Header("Fish")]
    public float fishXpGain;
    public string fishItemSlug;
    public int fishItemAmount;

    Inventory inventory;
    [HideInInspector] public List<Option> interactOptions { get; set; }

    void Start()
    {
        InitializeOptions();
        inventory = FindObjectOfType<Inventory>();
    }

    new public void InitializeOptions()
    {
        base.InitializeOptions();

        interactOptions = new List<Option>();
        interactOptions.AddRange(base.baseInteractOptions);
        interactOptions.Add(
            new Option() { Text = "Fish", OnOptionClick = delegate { base.MoveToInteractable(FishPond); } }
            );
        interactOptions.Format(this);
    }

    public void OpenContextMenu()
    {
        if (interactOptions != null)
        {
            InteractionManager.singleton.SetOptions(interactOptions, true);
            InteractionManager.singleton.Show();
        }
        else
        {
            Debug.LogError("Interact Options have not been set!");
        }
    }

    void FishPond()
    {
        Debug.Log("Fishing!");
    }
}