using UnityEngine;
using System.Collections.Generic;

[ItemCategory("stone")]
public class StoneItemType : ItemType, IItemType
{
    public List<Option> itemInteractOptions { get; set; }

    void Start()
    {
        InitializeOptions();
    }

    public override void InitializeOptions()
    {
        base.InitializeOptions();

        itemInteractOptions = new List<Option>();
        itemInteractOptions.AddRange(base.baseOptions);
        itemInteractOptions.Add(new Option()
        {
            Text = "Throw",
            OnOptionClick = Throw
        });
        itemInteractOptions.FormatItem(this);
    }

    public void ShowInteraction()
    {
        if (itemInteractOptions != null)
        {
            InteractionManager.singleton.SetOptions(itemInteractOptions, true);
            InteractionManager.singleton.Show();
        } else
        {
            Debug.LogError("Item Interaction Options must be set before used!");
        }
    }

    void Throw()
    {
        Debug.Log("Throwing Rock!");
        // TODO: Remove one rock from inventory.
    }
}