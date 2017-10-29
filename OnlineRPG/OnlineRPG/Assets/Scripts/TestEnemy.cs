using UnityEngine;

public class TestEnemy : MonoBehaviour, IEnemy
{
    public int Id { get; set; }
    public int Experience { get; set; }

    private Player player;

    void Start()
    {
        Id = 0;
    }

    public void PerformAttack()
    {

    }

    public void TakeDamage(float amount)
    {

    }

    public void Die()
    {

    }
}