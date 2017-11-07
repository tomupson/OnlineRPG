public class UltimateSlayerQuest : Quest
{
    void Start()
    {
        Name = "Ultimate Slayer";
        Description = "Kill a variety of enemies.";
        ItemReward = FindObjectOfType<ItemDatabase>().FetchItem("item_mahogany_logs");
        ItemRewardAmount = 5;
        ExperienceReward = 100;
        Goals.Add(new KillGoal(this, 0, "Kill 5 Skeletons", false, 2, 5));

        Goals.ForEach(g => g.Init());
    }
}