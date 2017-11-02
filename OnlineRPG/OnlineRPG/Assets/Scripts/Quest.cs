using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Quest : MonoBehaviour
{
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string Name { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public Item ItemReward { get; set; }
    public int ItemRewardAmount { get; set; } = 1;
    public bool Completed { get; set; }

    public void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        if (Completed) GiveReward();
    }

    void GiveReward()
    {
        if (ItemReward != null)
        {
            FindObjectOfType<Inventory>().AddItem(ItemReward, ItemRewardAmount);
        }
    }
}