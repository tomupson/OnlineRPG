using UnityEngine;
using System.Collections.Generic;

[ItemCategory("wood")]
public class WoodItemType : ItemType, IItemType
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
        itemInteractOptions.Add(new Option() { Text = "Light Fire", OnOptionClick = delegate
        {
            LightFire();
            InteractionManager.singleton.Hide();
        }
        });
        itemInteractOptions.FormatItem(this);
    }

    public void ShowInteraction()
    {
        InteractionManager.singleton.SetOptions(itemInteractOptions);
        InteractionManager.singleton.Show();
    }

    void LightFire()
    {
        Debug.Log("Lit fire");
    }
}