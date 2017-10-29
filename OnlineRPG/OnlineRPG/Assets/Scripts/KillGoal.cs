using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyId { get; set; }

    public KillGoal(int enemyId, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.EnemyId = enemyId;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(IEnemy enemy)
    {
        if (enemy.Id == this.EnemyId)
        {
            this.CurrentAmount++;
            base.Evaluate();
        }
    }
}