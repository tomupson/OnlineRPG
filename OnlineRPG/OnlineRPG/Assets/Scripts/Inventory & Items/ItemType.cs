using UnityEngine;
using System.Collections.Generic;

public class ItemType : MonoBehaviour
{
    public List<Option> baseOptions;
    public string itemName { get; set; }

    public virtual void InitializeOptions()
    {
        baseOptions = new List<Option>()
        {
            new Option()
            {
                Text = "Drop",
                OnOptionClick = DropItem
            },
            new Option()
            {
                Text = "Cancel",
                OnOptionClick = Cancel
            }
        };
    }

    void DropItem()
    {
        Debug.Log("Dropping Item");
    }

    void Cancel()
    {
        Debug.Log("Cancelled");
        InteractionManager.singleton.Hide();
    }
}