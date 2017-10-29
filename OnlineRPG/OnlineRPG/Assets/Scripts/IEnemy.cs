using UnityEngine;

public interface IEnemy
{
    int Id { get; set; }
    int Experience { get; set; }
    void Die();
    void TakeDamage(float amount);
    void PerformAttack();
}