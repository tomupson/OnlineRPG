using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractable : Interactable, IInteractable
{
    public List<Option> interactOptions { get; set; }

    void Start()
    {
        InitializeOptions();
    }

    new public void InitializeOptions()
    {
        base.InitializeOptions();

        interactOptions = new List<Option>();
        //interactOptions.AddRange(base.baseInteractOptions);
        interactOptions.Add(
            new Option()
            {
                Text = "View Inventory",
                OnOptionClick = delegate
                {
                    foreach (Item i in Inventory.singleton.items)
                    {
                        Debug.Log(i.Name);
                    }
                }
            });
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
}